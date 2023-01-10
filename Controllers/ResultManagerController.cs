using System.Net.Mime;
using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultManagerController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        public ResultManagerController(SqlContext sqlContext )
        {
            _sqlContext = sqlContext;
        }

        //validate selected answear
        [HttpPost]
        [Route("/SelectedResult/{RequestId}")]
        public ActionResult SelectedResult(int RequestId, [FromBody]TempOffer of)
        {
            try
            {
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                var res = _sqlContext.Responses.Where(r=>r.RequestID == RequestId && r.MonthlyInstallment == of.MonthlyInstallment).First();
                if (of.validate(req, res.MonthlyInstallment) && req.Status == RequestStatus.Pending.ToString())
                {
                    req.Status = RequestStatus.OfferSelected.ToString();
                    req.ResponseID = res.ResponseID;
                    res.State = ResponseStatus.PendingConfirmation.ToString();
                    _sqlContext.SaveChanges();
                    Helpers.sendUploadEmail(req.Email,req.RequestID);
                    return Ok();
                    //wyslac maila
                    //send add documents and send to review here
                    //return Redirect($"api/ResultManagement/getConfirmation/{req.RequestID}/{req.ResponseID}"); //to defaultowo ma iść w mailu, mail wysyłany dopiero po aprobacie admina
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("/getAgreement/{rqid}")]
        public async Task<ActionResult> GetDocumentUser(int rqid)
        {
            if (_sqlContext.Requests.Where(r =>r.RequestID == rqid).Count() == 1)
            {
                Request req = _sqlContext.Requests.Where(r =>  r.RequestID == rqid).First();
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                if (res.External)
                {
                    //TODO hit external api for result
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response3 = await client.GetAsync($"{res.ApiInfo.Split("&&&")[1]}/Offer/{res.OfferId.ToString()}/document/{res.documentKey.Split('/').Last()}");
                    return File(response3.Content.ReadAsStream(), MediaTypeNames.Text.Plain, "File.txt");
                    //var stream = 
                    //return Ok();
                }
                else
                {
                    var stream = Helpers.generateAgreeement(req.Name, req.Surname);
                    return File(stream, MediaTypeNames.Text.Plain, "document.txt"); 
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("/UploadAgreement/{RequestId}")]
        public async Task<ActionResult> UploadAgreement(int RequestId,  IFormFile file)
        {
            string tempDirectoryPath = (string)Environment.GetEnvironmentVariable("TEMP");
            string upload = Path.Combine(tempDirectoryPath, "upload");
            try
            {

                string newname = $"{Guid.NewGuid()}_{file.FileName}";
                using (Stream contentStream = file.OpenReadStream())
                {
                    Helpers.uploadDocument("dotnet-bank-agreements",contentStream,newname);
                }
                //Helpers.uploadDocument("dotnet-bank-agreements",filePath,newname);
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                var res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                if (res.External)
                {
                    //TODO send doc to external api as well
                    using (var form = new MultipartFormDataContent())
                    {
                        using(var client = new HttpClient())
                        {
                            using (Stream contentStream = file.OpenReadStream())
                            {
                                form.Add(new StreamContent(contentStream),"Agreement.txt");
                                var response = await client.PostAsync($"{res.ApiInfo.Split("&&&")[1]}/Offer/{res.OfferId}/document/upload",form);
                            }
                            
                            //return await response.Content.ReadAsStringAsync();
                        }
                    }
                    
                    req.AgreementKey = newname;
                    req.Status = RequestStatus.DocumentsProvided.ToString();
                    Helpers.sendInitalStatusUpdateEmail(req.Email);
                    _sqlContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    req.AgreementKey = newname;
                    req.Status = RequestStatus.DocumentsProvided.ToString();
                    Helpers.sendInitalStatusUpdateEmail(req.Email);
                    _sqlContext.SaveChanges();
                    return Ok();
                }
                
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
                //Console.WriteLine(e);
                throw;
            }
            
        }


        //INVALID
        [HttpPost]
        [Route("/UploadDocument/{RequestId}")]
        public ActionResult UploadDocument(int RequestId,  IFormFile file)
        {
            string tempDirectoryPath = (string)Environment.GetEnvironmentVariable("TEMP");
            string upload = Path.Combine(tempDirectoryPath, "upload");
            try
            {
                
                string newname = $"{Guid.NewGuid()}_{file.FileName}";
                using (Stream contentStream = file.OpenReadStream())
                {
                    Helpers.uploadDocument("dotnet-bank-documents",contentStream,newname);
                }
                //Helpers.uploadDocument("dotnet-bank-documents",filePath,newname);
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                req.DocumentKey = newname;
                if (req.AgreementKey is not null)
                {
                    req.Status = RequestStatus.DocumentsProvided.ToString();
                    //Helpers.sendInitalStatusUpdateEmail(req.Email);
                    //wyslac mail
                }
                _sqlContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
                //Console.WriteLine(e);
                throw;
            }
        }

        //final confirmation
        [HttpGet]
        [Route("/getFinalConfirmation/{rqid}/{guid}")]
        public async Task<ActionResult> GetConfirmation(int rqid,Guid guid)
        {
            if (_sqlContext.Requests.Where(r => r.MappedGuid == guid && r.RequestID == rqid).Count() == 1)
            {
                Request req = _sqlContext.Requests.Where(r => r.MappedGuid == guid && r.RequestID == rqid).First();
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                res.State = Data.ResponseStatus.FinalApproved.ToString();
                req.Status = Data.RequestStatus.FinalApproved.ToString();
                _sqlContext.SaveChanges();
                if (res.External)
                {
                    HttpClient client = new HttpClient();
                    var response = await client.PostAsync($"{res.ApiInfo.Split("&&&")[1]}/Offer/{res.OfferId.ToString()}/complete",null);
                    //TODO send complete to external api
                }
                return Ok(Response);
            }
            else
            {
                return NotFound();
            }
        }
    }

    
}
