using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIConnectorController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        private string _decodedAuthData;
        private string _extensionAppID;

        public APIConnectorController(SqlContext sqlContext, IConfiguration configuration)
        {
            _sqlContext = sqlContext;

            string username = configuration.GetValue<string>("APIConnector:Username");
            string password = configuration.GetValue<string>("APIConnector:Password");

            _decodedAuthData = username + ":" + password;

            _extensionAppID = configuration.GetValue<string>("b2cExtensionApp:ClientId");
        }

        [HttpPost("NewUser")]
        public ActionResult<BaseAPIConnectorResponse> ApproveNewUser([FromBody] Dictionary<string, string> Data)
        {
            //Authorization
            var authHeader = Request.Headers.Authorization;
            if (ApiConnectorAuth(Request.Headers.Authorization) == false)
                return Ok(new APIConnectorBlock("Invalid auth data"));

            try
            {
                User newUser = new User();
                newUser.Name = Data["givenName"];
                newUser.Surname = Data["surname"];
                newUser.GovermentId = Data["extension_" + _extensionAppID + "_GovermentID"];
                newUser.Email = Data["email"];
                newUser.JobType = Data["extension_" + _extensionAppID + "JobType"];
                newUser.IncomeLevel = decimal.Parse(Data["extension_" + _extensionAppID + "_Incomelevel"]);

                _sqlContext.Users.Add(newUser);
                _sqlContext.SaveChanges();

                return Ok(new APIConnectorContinue());
            }
            catch
            {
                return Ok(new APIConnectorBlock("There was a problem with your request. Try again"));
            }
        }

        private bool ApiConnectorAuth(StringValues authHeader)
        {
            if (StringValues.IsNullOrEmpty(authHeader))
                return false;

            if (authHeader.Count != 2)
                return false;

            if (authHeader[0] != "Basic")
                return false;

            byte[] decodedData = Convert.FromBase64String(authHeader[1]);
            if (System.Text.Encoding.UTF8.GetString(decodedData) != _decodedAuthData)
                return false;

            return true;
        }
    }
}
