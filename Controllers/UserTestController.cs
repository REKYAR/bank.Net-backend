using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;
using Azure.Communication.Email.Models;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTestController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        public UserTestController(SqlContext sqlContext )
        {
            _sqlContext = sqlContext;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _sqlContext.Users;
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
        [Authorize]
        [HttpGet("ProfileInfo")]
        public ActionResult<User> LoggedUserInfo()
        {
            var claims = User.Claims.ToList();

            var NameClaim = GetClaim(claims, ClaimTypes.GivenName);
            var SurnameClaim = GetClaim(claims, ClaimTypes.Surname);
            var EmailClaim = GetClaim(claims, "emails");
            var IncomeClaim = GetClaim(claims, "extension_Incomelevel");
            var GovermentIDClaim = GetClaim(claims, "extension_GovermentID");
            var JobTypeClaim = GetClaim(claims, "extension_JobType");

            if (NameClaim == null || SurnameClaim == null || EmailClaim == null ||
                IncomeClaim == null || GovermentIDClaim == null || JobTypeClaim == null)
            {
                return NotFound();
            }

            var user = new User();

            user.Surname = SurnameClaim.Value;
            user.Name = NameClaim.Value;
            user.Email = EmailClaim.Value;
            user.GovermentId = GovermentIDClaim.Value;
            user.JobType = JobTypeClaim.Value;
            user.IncomeLevel = decimal.Parse(IncomeClaim.Value);

            return Ok(user);
        }

        private Claim? GetClaim(List<Claim> claims, string claimType)
        {
            return claims.Find(x => x.Type == claimType);
        }

        [HttpPost]
        public ActionResult<User> PostExampleUser()
        {
            var user = new User();
            user.IncomeLevel = 1;
            user.Surname = "Lastname";
            user.Name = "Name";
            user.Email = "useremail@xd.com";
            user.GovermentId = "123";
            user.JobType = "TestJob";
            _sqlContext.Users.Add(user);
            _sqlContext.SaveChanges();
            return Ok( user );
        }

        [HttpGet("TestEmail")]
        public ActionResult<string> Email()
        {
            try
            {
                SendEmailResult ser =  Helpers.sendTextEmail();
                return Ok( ser.ToString() );
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message} ||| e.InnerException.Message");
                throw;
            }
            
           
        }
    }
}
