using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;


namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestManagementController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        public RequestManagementController(SqlContext sqlContext )
        {
            _sqlContext = sqlContext;
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<CompleteRequest>> GetAllRequests()
        {
            var claims = User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if ( EmailClaim == null)
            {
                return NotFound();
            }
            User? u = _sqlContext.Users.Where(u => u.Email == EmailClaim.Value).First();
            if ( u == null)
            {
                return NotFound();
            }
            
            List<Data.Request> reqs = _sqlContext.Requests.Where(req => req.User == u).ToList();
            List<RequestDTO> dto =  new List<RequestDTO>();
            foreach (Request r in reqs)
            {
                dto.Add(new RequestDTO(r.Date,r.Amount,r.NumberOfInstallments,r.Name,r.Surname,r.GovermentId,r.Email,r.JobType,r.IncomeLevel,r.Status));
            }

            return Ok(dto);
        }

        //create request and redirect to offers
        //spr zrobic request
        [HttpPost]
        //public ActionResult PostRequest([FromBody] Dictionary<string, string> Requestdata)
        public ActionResult PostRequest([FromBody] RequestDTO Requestdata)
        {
            Data.Request req = new Data.Request();
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
                //req.Date = DateTime.UtcNow;
                //req.RequestID = 0;
                //req.Status = RequestStatus.Pending.ToString();

                if (_sqlContext.Users.Where(u => u.Email == req.Email).Count() != 0)
                {
                    User u = _sqlContext.Users.Where(u => u.Email == req.Email).First();
                    req.User = u;
                    u.Requests.Add(req);
                }
                _sqlContext.Requests.Add(req);
            }
            catch (Exception c)
            {

                return BadRequest(c);
            }
            //_sqlContext.Requests.Add(req);
            //send mail with request id
            return Redirect($"api/requestManagement/offers/{req.RequestID}");
            //return Ok();
        }

        //generate 3 offers (for now we generate all 3)
        [HttpGet]
        [Route("api/requestManagement/offers/{RequestID}")]
        public ActionResult<IEnumerable<Offer>> getOffers(int RequestID)
        {
            if (_sqlContext.Requests.Where(r => r.RequestID == RequestID).Count() == 1)
            {
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
                List<Offer> offers = new List<Offer>();
                offers.Add(new Offer(req,Logic.generateOffer(req)));
                offers.Add(new Offer(req,Logic.generateOffer(req)));
                offers.Add(new Offer(req,Logic.generateOffer(req)));
                foreach (Offer offer in offers)
                {
                    Response r = new Response();
                    r.Request = req;
                    r.User = req.User;
                    r.UserEmail = req.Email;
                    r.MonthlyInstallment = offer.MonthlyInstallment;
                    r.State = ResponseStatus.PendingApproval.ToString();
                    _sqlContext.Responses.Add(r);
                }
                return Ok(offers);
            }
            else
            {
                return NotFound();
            }
        }

        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        //[HttpPost]
        //[Authorize]
        //public ActionResult PostRequest([FromBody] Dictionary<string, string> Requestdata)
        //{

        //}
    }
}
