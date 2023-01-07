using System.Net.Mime;
using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly SqlContext _sqlContext;

        public ExternalController(SqlContext sqlContextX)
        {
            this._sqlContext = sqlContextX;
        }

        //post inqure (create request)
        [HttpPost("api/[controller]/postInquire")]
        public ActionResult<int> PostRequestExternal([FromBody] RequestDTO Requestdata)
        {
            Data.Request req = new Data.Request();
            Data.Response res = new Data.Response();
            try
            {

                req.IncomeLevel = Requestdata.IncomeLevel;
                req.Status = RequestStatus.Pending.ToString();
                req.Surname = Requestdata.Surname;
                req.Date = DateTime.UtcNow;
                req.Amount = Requestdata.Amount;
                req.Email = Requestdata.Email;
                req.GovermentId = Requestdata.GovermentId;
                req.NumberOfInstallments = Requestdata.NumberOfInstallments;
                req.IncomeLevel = Requestdata.IncomeLevel;
                req.Name = Requestdata.Name;
                req.JobType = Requestdata.JobType;
                req.MappedGuid = new Guid();
                req.External = true;
                req.ApiInfo = Requestdata.ApiInfo;

                _sqlContext.Requests.Add(req);
                decimal moInstallment = Logic.generateOffer(req);

                //res.RequestID = req.RequestID;
                res.MonthlyInstallment = moInstallment;
                res.UserEmail = req.Email;
                res.State = ResponseStatus.PendingApproval.ToString();
                _sqlContext.Responses.Add(res);
                
                _sqlContext.SaveChanges();
                req.ResponseID = res.ResponseID;
                res.RequestID= req.RequestID;
                _sqlContext.SaveChanges();
            }
            catch (Exception c)
            {
                return BadRequest(c.Message);
            }

            //_sqlContext.Requests.Add(req);
            //send mail with request id
            return Ok(req.RequestID);


        }

        //get inqure 
        [HttpGet("getInquire/{RequestID}")]
        public ActionResult<Request> GetRequestExternal(int RequestID)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
                return Ok(req);
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        //get offer
        [HttpGet("offer/{ResponseId}")]
        public ActionResult<Response> GetResultExternal(int ResponseId)
        {
            try
            {
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == ResponseId).First();
                return Ok(res);
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        //get offer document
        [HttpPost("getAgreement/{RequestID}")]
        public ActionResult GetDocumentTemplateExternal(int RequestID)
        {
            try
            {
                Request r = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
                var stream = Helpers.generateAgreeement(r.Name, r.Surname);
                //Response res = _sqlContext.Responses.Where(r => r.ResponseID == ResponseId).First();
                return File(stream, MediaTypeNames.Application.Pdf, "document.pdf");
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        //post offer document
        [HttpPost]
        [Route("uploadAgreement/{RequestId}")]
        public ActionResult UploadDocumentExternal(int RequestId, IFormFile file)
        {
            string tempDirectoryPath = (string)Environment.GetEnvironmentVariable("TEMP");
            string upload = Path.Combine(tempDirectoryPath, "upload");
            try
            {

                string newname = $"{Guid.NewGuid()}_{file.FileName}";
                using (Stream contentStream = file.OpenReadStream())
                {
                    Helpers.uploadDocument("dotnet-bank-agreements", contentStream, newname);
                }

                //Helpers.uploadDocument("dotnet-bank-documents",filePath,newname);
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                req.DocumentKey = newname;
                req.Status = RequestStatus.DocumentsProvided.ToString();
                //Helpers.sendInitalStatusUpdateEmail(req.Email);

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

        //finalize request
        [HttpGet]
        [Route("offer/{RequestID}/complete")]
        public ActionResult FinalizeExternal(int RequestID)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                req.Status = RequestStatus.ExternalClosed.ToString();
                res.State = ResponseStatus.ExternalClosed.ToString();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
