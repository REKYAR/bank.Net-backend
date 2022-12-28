using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        [HttpPost("/generateOffer")]
        public ActionResult<Offer> PostRequest([FromBody] RequestDTO request)
        {
            try
            {
                decimal percentage = Logic.generateOffer(request);
                Offer offer = new Offer(request, percentage);
                return Ok(offer);
            }
            catch (Exception e)
            {
                return BadRequest();
                //Console.WriteLine(e);
                //throw;
            }
            

        }
    }
}
