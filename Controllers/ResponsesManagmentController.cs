using System.Net.Mime;
using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Bank.NET___backend.Models.QueryParametres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesManagmentController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        public ResponsesManagmentController(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        [HttpGet]
        //[Authorize(Policy = "BankEmployee")]
        public ActionResult<IEnumerable<CompleteRequest>> GetResponses([FromQuery] ResponseQueryParameters parametres)
        {
            if (!parametres.ValidateParameters())
            {
                return BadRequest();
            }

            //IQueryable<CompleteRequest> data = _sqlContext.Responses.Join(_sqlContext.Requests, res => res.RequestID, req => req.RequestID,
            //    (res, req) => new CompleteRequest(res, req));

            var data = from request in _sqlContext.Requests
                       join response in _sqlContext.Responses
                       on request.ResponseID equals response.ResponseID
                       select new CompleteRequest(response, request);

            return Ok(parametres.handleQueryParametres(data));
        }

        [RequiredScope("access_as_user")]
        [HttpPost("Approve")]
        [Authorize(Policy = "BankEmployee")]
        public ActionResult<ResponseDTO> ApproveResponse([FromBody] Dictionary<string, string> ResponseData)
        {
            try
            {
                decimal MonthlyInstallments = decimal.Parse(ResponseData["MonthlyInstallments"]);
                string mail = ResponseData["UserEmail"];

                List <Response> responses = _sqlContext.Responses.Where(res =>
                    res.UserEmail == mail && res.MonthlyInstallment == MonthlyInstallments && res.State == ResponseStatus.PendingConfirmation.ToString())
                    .ToList();

                if ( responses.Count > 0)
                {
                    responses[0].State = ResponseStatus.Approved.ToString();

                    return Ok(responses[0]);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [RequiredScope("access_as_user")]
        [HttpPost("Refuse")]
        [Authorize(Policy = "BankEmployee")]
        public ActionResult<ResponseDTO> RefuseResponse([FromBody] Dictionary<string, string> ResponseData)
        {
            try
            {
                decimal MonthlyInstallments = decimal.Parse(ResponseData["MonthlyInstallments"]);
                string mail = ResponseData["UserEmail"];

                List<Response> responses = _sqlContext.Responses.Where(res =>
                    res.UserEmail == mail && res.MonthlyInstallment == MonthlyInstallments && res.State == ResponseStatus.PendingConfirmation.ToString())
                    .ToList();

                if (responses.Count > 0)
                {
                    responses[0].State = ResponseStatus.Refused.ToString();

                    return Ok(responses[0]);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("GetAgreement/{rqid}")]
        [Authorize(Policy = "BankEmployee")]
        [RequiredScope("access_as_user")]
        public ActionResult GetAgreement(int rqid)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.RequestID == rqid).First();
                ;
                var stream = Helpers.downloadDocument("dotnet-bank-agreements",req.AgreementKey);
                stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "agreement.jpg");
                //MediaTypeNames.Image.Jpeg.ToString();
                return File(stream, MediaTypeNames.Text.Plain,"agreement.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        [HttpPost("GetDocument/{rqid}")]
        [Authorize(Policy = "BankEmployee")]
        [RequiredScope("access_as_user")]
        public ActionResult GetDocument(int rqid)
        {
            try
            {
                Request req = _sqlContext.Requests.Where(r => r.RequestID == rqid).First();
                var stream = Helpers.downloadDocument("dotnet-bank-documents",req.DocumentKey);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, MediaTypeNames.Image.Jpeg, "document.jpg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private List<ResponseDTO> CreateResponseRTO(List<Response> responses)
        {
            List<ResponseDTO> result = new List<ResponseDTO>(responses.Count);
            foreach (var resp in responses)
            {
                result.Add(new ResponseDTO(resp.State, resp.MonthlyInstallment, resp.UserEmail));
            }

            return result;
        }
    }
}
