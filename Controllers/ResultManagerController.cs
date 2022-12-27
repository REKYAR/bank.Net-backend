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
        public ActionResult SelectedResult(int RequestId, [FromBody]Offer of)
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
                    return Ok();
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

        [HttpPost]
        [Route("/UploadAgreement/{RequestId}")]
        public ActionResult UploadAgreement(int RequestId,  IFormFile file)
        {
            string tempDirectoryPath = (string)Environment.GetEnvironmentVariable("TEMP");
            string upload = Path.Combine(tempDirectoryPath, "upload");
            try
            {
                //var files = Request.Form.Files;
                //if (files.Count != 1)
                //{
                //    return BadRequest("more/less than one file uploaded");
                //}

                //IFormFile file = files.First();
                string newname = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = $".\\uploads\\{newname}";
                using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }
                Helpers.uploadDocument("dotnet-bank-agreements",filePath,newname);
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                req.AgreementKey = newname;
                if (req.DocumentKey is not null)
                {
                    req.Status = RequestStatus.DocumentsProvided.ToString();
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

        [HttpPost]
        [Route("/UploadDocument/{RequestId}")]
        public ActionResult UploadDocument(int RequestId,  IFormFile file)
        {
            string tempDirectoryPath = (string)Environment.GetEnvironmentVariable("TEMP");
            string upload = Path.Combine(tempDirectoryPath, "upload");
            try
            {
                //var files = Request.Form.Files;
                //if (files.Count != 1)
                //{
                //    return BadRequest("more/less than one file uploaded");
                //}

                //IFormFile file = files.First();
                string newname = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = Path.Combine(upload, newname);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }
                Helpers.uploadDocument("dotnet-bank-documents",filePath,newname);
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                req.DocumentKey = newname;
                if (req.AgreementKey is not null)
                {
                    req.Status = RequestStatus.DocumentsProvided.ToString();
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
        [Route("/getConfirmation/{rqid}/{rsid}")]
        public ActionResult GetConfirmation(int rqid, int rsid)
        {
            if (_sqlContext.Responses.Where(r => r.ResponseID == rsid).Count() == 1 && _sqlContext.Requests.Where(r => r.RequestID== rqid).Count() == 1)
            {
                var res = _sqlContext.Responses.Where(r => r.ResponseID == rsid).First();
                res.State = ResponseStatus.Approved.ToString();
                return Redirect("/");
            }
            else
            {
                return NotFound();
            }
        }
    }

    
}
