using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
