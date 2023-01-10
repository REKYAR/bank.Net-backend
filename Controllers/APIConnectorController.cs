using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIConnectorController : ControllerBase
    {
        private static string authPrefix = "Basic ";
        
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
        public ActionResult<BaseAPIConnectorResponse> ApproveNewUser([FromBody] Dictionary<string, JsonElement> Data)
        {
            var authHeader = Request.Headers.Authorization;
            if (ApiConnectorAuth(Request.Headers.Authorization) == false)
                return Ok(new APIConnectorBlock("Invalid auth data"));

            try
            {
                User newUser = new User();
                newUser.Name = Data["givenName"].ToString();
                newUser.Surname = Data["surname"].ToString();
                newUser.GovermentId = Data["extension_" + _extensionAppID + "_GovermentID"].ToString();
                newUser.Email = Data["email"].ToString();
                newUser.JobType = Data["extension_" + _extensionAppID + "_JobType"].ToString();
                newUser.IncomeLevel = Data["extension_" + _extensionAppID + "_Incomelevel"].GetDecimal();

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

            if (authHeader.Count != 1)
                return false;

            byte[] decodedData = Convert.FromBase64String(authHeader[0].Substring(authPrefix.Length));
            if (System.Text.Encoding.UTF8.GetString(decodedData) != _decodedAuthData)
                return false;

            return true;
        }
    }
}
