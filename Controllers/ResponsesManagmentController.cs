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
            PagedList<Response> responses = PagedList<Response>.ToPagedList(_sqlContext.Responses.OrderBy(res => res.RequestID),
                parametres.PageNumber,
                parametres.PageSize);
              
            List<ResponseDTO> result = CreateResponseRTO(responses);

            Response.Headers.Add("Pagination", responses.getMetadata());

            return Ok(result);
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        [HttpGet("Pending")]
        [Authorize(Policy = "BankEmployee")]
        public ActionResult<IEnumerable<CompleteRequest>> GetPendingConfirmationResponses()
        {
            List<Response> responses = _sqlContext.Responses.Where(res => res.State == ResponseStatus.PendingConfirmation.ToString()).ToList();
            List<ResponseDTO> result = CreateResponseRTO(responses);

            return Ok(result);
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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
