using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIConnectorController : ControllerBase
    {
        private readonly SqlContext _sqlContext;

        public APIConnectorController(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        [HttpPost("NewUser")]
        public ActionResult<BaseAPIConnectorResponse> ApproveNewUser([FromBody] Dictionary<string, string> Data)
        {
            try
            {
                User newUser = new User();
                newUser.Name = Data["givenName"];
                newUser.Surname = Data["surname"];
                newUser.GovermentId = Data["extension_GovermentID"];
                newUser.Email = Data["email"];
                newUser.JobType = Data["extension_JobType"];
                newUser.IncomeLevel = decimal.Parse(Data["extension_Incomelevel"]);

                _sqlContext.Users.Add(newUser);
                _sqlContext.SaveChanges();

                return Ok(new APIConnectorContinue());
            }
            catch
            {
                return Ok(new APIConnectorBlock("There was a problem with your request. Try again"));
            }
        }
    }
}
