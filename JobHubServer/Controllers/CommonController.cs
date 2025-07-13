using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpGet("ping")]
        public ActionResult Status()
        {
            return Ok("Hello from JobHub server!");
        }
    }
}
