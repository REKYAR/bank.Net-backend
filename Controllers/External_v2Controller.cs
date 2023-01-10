using System.Net.Mime;
using Bank.NET___backend.ApiStructures.NewFolder.bankapi4dotnet;
using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class External_v2Controller : ControllerBase
    {
        private readonly SqlContext _sqlContext;

        public External_v2Controller(SqlContext sqlContextX)
        {
            this._sqlContext = sqlContextX;
        }

        //post inqure (create request)
        [HttpPost("/api/[controller]/Inquire")]
        public ActionResult<int> PostRequestExternal([FromBody] CreateInquiryRequest Requestdata)
        {
            Data.Request req = new Data.Request();
            Data.Response res = new Data.Response();
            
            try
            {

                req.IncomeLevel = Requestdata.IncomeLevel;
                req.Status = RequestStatus.Pending.ToString();
                req.Surname = Requestdata.LastName;
                req.Date = DateTime.UtcNow;
                req.Amount = Requestdata.MoneyAmount;
                req.Email = "placeholder_email";
                req.GovermentId = $"{Requestdata.DocumentType}&&&{Requestdata.DocumentId}";
                req.NumberOfInstallments = Requestdata.InstallmentsNumber;
                req.IncomeLevel = Requestdata.IncomeLevel;
                req.Name = Requestdata.FirstName;
                req.JobType = $"{Requestdata.JobType}";
                req.MappedGuid = Guid.NewGuid();
                req.External = true;
                //req.ApiInfo = Requestdata.ApiInfo;

                Inquiry inq = new Inquiry(Guid.NewGuid(),DateTime.UtcNow, req.Amount,req.NumberOfInstallments,req.Name,req.Surname, Requestdata.DocumentType, Requestdata.DocumentId, Requestdata.JobType, Requestdata.IncomeLevel);
                req.InquireId = inq.Id;
                _sqlContext.Inquiries.Add(inq);
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
        [HttpGet("/api/[controller]/Inquire/{InquireId}")]
        public ActionResult<CreateInquiryResponse> GetRequestExternal(Guid InquireId)
        {
            try
            {
                Inquiry inq = _sqlContext.Inquiries.Where(i => i.Id == InquireId).First();
                Request req = _sqlContext.Requests.Where(r => r.InquireId == InquireId).First();
                return Ok(new CreateInquiryResponse(inq.Id, inq.CreationDate, (Guid)req.MappedGuid));
            }
            catch (Exception e)
            {
                return NotFound();
            }
            //try
            //{
            //    Request req = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
            //    return Ok(req);
            //}
            //catch (Exception e)
            //{
            //    return NotFound();
            //}

        }

        //get offer
        [HttpGet("/api/[controller]/Offer/{OfferId}")]
        public ActionResult<ApiStructures.NewFolder.bankapi4dotnet.Offer> GetResultExternal(Guid OfferId)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.MappedGuid == OfferId).First();
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                return Ok(new ApiStructures.NewFolder.bankapi4dotnet.Offer((Guid)req.MappedGuid,
                    (int)((res.MonthlyInstallment / (req.Amount / req.NumberOfInstallments) - 1) * 100),
                    res.MonthlyInstallment, req.Amount, req.NumberOfInstallments, 1, req.Status, (Guid)req.InquireId,
                    req.Date, req.Date,"", DateTime.Parse("9999-12-31T23:59:59.9999999")));
                    //return Ok(res);
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        //get offer document
        [HttpPost("/api/[controller]/Offer/{OfferId}/document/{key}")]
        public ActionResult GetDocumentTemplateExternal(Guid OfferId, string key)
        {
            try
            {
                Request r = _sqlContext.Requests.Where(r => r.MappedGuid == OfferId).First();
                var stream = Helpers.generateAgreeement(r.Name, r.Surname);
                //Response res = _sqlContext.Responses.Where(r => r.ResponseID == ResponseId).First();
                return File(stream, MediaTypeNames.Text.Plain, "document.txt");
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        //post offer document
        [HttpPost]
        [Route("/api/[controller]/Offer/{OfferId}/document/upload")]
        public ActionResult UploadDocumentExternal(Guid OfferId, IFormFile file)
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
                var req = _sqlContext.Requests.Where(r => r.MappedGuid == OfferId).First();
                req.AgreementKey = newname;
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
        [Route("/api/[controller]/Offer/{OfferId}/complete")]
        public ActionResult FinalizeExternal(Guid OfferId)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.MappedGuid == OfferId).First();
                Response res = _sqlContext.Responses.Where(r => r.ResponseID == req.ResponseID).First();
                req.Status = RequestStatus.ExternalClosed.ToString();
                res.State = ResponseStatus.ExternalClosed.ToString();
                _sqlContext.SaveChanges();
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
