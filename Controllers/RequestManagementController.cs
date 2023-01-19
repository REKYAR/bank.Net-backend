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

        /*[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        [HttpGet]
        [Route("/getAllRequests")]
        [Authorize]
        public ActionResult<IEnumerable<CompleteRequest>> GetAllRequests()
        {
            List<Data.Request> reqs = _sqlContext.Requests.ToList();
        }*/

        [RequiredScope("access_as_user")]
        [HttpGet]
        [Route("/getUserRequests")]
        [Authorize]
        public ActionResult<IEnumerable<CompleteRequest>> GetUserRequests()
        {
            var claims = User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if ( EmailClaim == null)
            {
                return NotFound();
            }
            User? u = _sqlContext.Users.Where(u => u.Email == EmailClaim.Value).FirstOrDefault(defaultValue:null);
            if ( u == null)
            {
                return NotFound();
            }
            
            
            try
            {
                List<Data.Request> reqs = _sqlContext.Requests.Where(req => req.UserID == u.UserID).ToList();
                List<CompleteRequest> dto =  new List<CompleteRequest>();
                foreach (Request r in reqs)
                {
                    if (r.ResponseID is null)
                    {
                        dto.Add(new CompleteRequest(r, null));
                    }
                    else
                    {
                        dto.Add(new CompleteRequest(r, _sqlContext.Responses.Where(res => res.ResponseID == r.ResponseID).First().MonthlyInstallment));
                    }
               
                }

                return Ok(dto);
            }
            catch (Exception e)
            {
                return new List<CompleteRequest>();
                Console.WriteLine(e);
                throw;
            }
            
        }

        [RequiredScope("access_as_user")]
        [HttpGet]
        [Route("/getRecentRequests")]
        [Authorize]
        public ActionResult<IEnumerable<CompleteRequest>> GetLast30DaysRequests()
        {
            var claims = User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if ( EmailClaim == null)
            {
                return NotFound();
            }
            User? u = _sqlContext.Users.Where(u => u.Email == EmailClaim.Value).FirstOrDefault(defaultValue:null);
            if ( u == null)
            {
                return NotFound();
            }

            try
            {
                List<Data.Request> reqs = _sqlContext.Requests.Where(req => req.UserID == u.UserID && (DateTime.UtcNow - req.Date ).Days <= 30).ToList();
                List<CompleteRequest> dto =  new List<CompleteRequest>();
                foreach (Request r in reqs)
                {
                    if (r.ResponseID is null)
                    {
                        dto.Add(new CompleteRequest(r, null));
                    }
                    else
                    {
                        dto.Add(new CompleteRequest(r, _sqlContext.Responses.Where(res => res.ResponseID == r.ResponseID).First().MonthlyInstallment));
                    }
                }

                return Ok(dto);
            }
            catch (Exception e)
            {
                return new List<CompleteRequest>();
                Console.WriteLine(e);
                throw;
            }
            
        }

        [HttpGet]
        [Route("/inspect/{rqid}")]
        public ActionResult<CompleteRequest> GetRequest(int rqid)
        {
            try
            {
                Request r = _sqlContext.Requests.Where(r => r.RequestID == rqid).First();
                if (r.ResponseID is null)
                {
                    return Ok(new CompleteRequest(r, null));
                }
                else
                {
                    return Ok(new CompleteRequest(r, _sqlContext.Responses.Where(res => res.ResponseID == r.ResponseID).First().MonthlyInstallment));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                req.JobType = Requestdata.JobType;
                req.MappedGuid = Guid.NewGuid();
                req.ApiInfo = null;
                req.External = false;
                if (_sqlContext.Users.Where(u => u.Email == req.Email).Count() != 0)
                {
                    User u = _sqlContext.Users.Where(u => u.Email == req.Email).First();
                    req.UserID = u.UserID;
                }
                _sqlContext.Requests.Add(req);
                _sqlContext.SaveChanges();
            }
            catch (Exception c)
            {
                return BadRequest(c.Message);
            }
            //_sqlContext.Requests.Add(req);
            //send mail with request id
            Helpers.sendLinkToReq(req.Email,req.RequestID);
            return  Ok(req.RequestID);
            //return Redirect($"https://bank-project-backend-dev.azurewebsites.net/api/requestManagement/offers/{req.RequestID}");
            //return Ok();
        }

        //generate 3 offers (for now we generate all 3)
        [HttpGet]
        [Route("api/RequestManagement/offers/{RequestID}")]
        public async Task<ActionResult<IEnumerable<TempOffer>>> getOffers(int RequestID)
        {
            if (_sqlContext.Requests.Where(r => r.RequestID == RequestID).Count() == 1)
            {
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestID).First();
                List<TempOffer> offers = new List<TempOffer>();
                offers.Add(new Models.TempOffer(req,Logic.generateOffer(req)));
                offers.Add(new Models.TempOffer(req,Logic.generateOffer(req)));//to be changed
                offers.Add(new Models.TempOffer(req,Logic.generateOffer(req)));//to be changed
                //own
                Response r1 = new Response();
                r1.RequestID = req.RequestID;
                r1.External = false;
                //r.User = req.User;
                r1.UserEmail = req.Email;
                r1.MonthlyInstallment = offers[0].MonthlyInstallment;
                r1.State = ResponseStatus.PendingApproval.ToString();
                _sqlContext.Responses.Add(r1);


                //external1 TODO change to integrate with external
                Response r2 = new Response();
                r2.RequestID = req.RequestID;
                r2.External = false;
                //r.User = req.User;
                r2.UserEmail = req.Email;
                r2.MonthlyInstallment = offers[1].MonthlyInstallment;
                r2.State = ResponseStatus.PendingApproval.ToString();
                _sqlContext.Responses.Add(r2);
                //external2
                string in1 = System.Environment.GetEnvironmentVariable("API_1");
                //string in1 = "uri&&&https://bankapi4dotnet.azurewebsites.net/";
                Response r3 = await Helpers.GetOfferFromApi1(req, in1);
                offers[2].MonthlyInstallment = r3.MonthlyInstallment;
                _sqlContext.Responses.Add(r3);
                //foreach (Offer offer in offers)
                //{
                //    Response r = new Response();
                //    r.RequestID = req.RequestID;
                //    r.External = false;
                //    //r.User = req.User;
                //    r.UserEmail = req.Email;
                //    r.MonthlyInstallment = offer.MonthlyInstallment;
                //    r.State = ResponseStatus.PendingApproval.ToString();
                //    _sqlContext.Responses.Add(r);
                //}
                _sqlContext.SaveChanges();
                return Ok(offers);
            }
            else
            {
                return NotFound(RequestID);
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
