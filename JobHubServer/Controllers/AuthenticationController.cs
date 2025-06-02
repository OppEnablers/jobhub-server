using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController() : ControllerBase
    {
        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.DefaultInstance;

        [Authorize]
        [HttpGet("login")]
        public ActionResult login()
        {
            return Ok();
        }

        [HttpGet("boombadablingbling")]
        public async Task<ActionResult<string>> DebugGrabToken()
        {
            return await _firebaseAuth.CreateCustomTokenAsync("debug-server");
        }
    }
}
