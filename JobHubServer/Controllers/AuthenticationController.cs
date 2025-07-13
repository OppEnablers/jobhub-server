using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(FirestoreDb firestore) : ControllerBase
    {
        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.DefaultInstance;

        [Authorize]
        [HttpGet("login")]
        public ActionResult Login()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("signup/jobseeker")]
        public async Task<ActionResult> SignupJobSeeker([FromBody] JobSeeker jobSeeker)
        {
            await _firebaseAuth.SetCustomUserClaimsAsync(jobSeeker.UserId, SetClaims("jobseeker"));
            await firestore.Collection("jobseekers").Document(jobSeeker.UserId).SetAsync(jobSeeker);

            return Ok();
        }

        [HttpPost("signup/employer")]
        public async Task<ActionResult> SignupEmployer([FromBody] Employer employer)
        {
            await _firebaseAuth.SetCustomUserClaimsAsync(employer.UserId, SetClaims("employer"));
            await firestore.Collection("employers").Document(employer.UserId).SetAsync(employer);

            return Ok();
        }

        [HttpGet("boombadablingbling")]
        public async Task<ActionResult<string>> DebugGrabToken()
        {
            return await _firebaseAuth.CreateCustomTokenAsync("debug-server");
        }

        private IReadOnlyDictionary<string, object> SetClaims(string userType)
        {
            return new Dictionary<string, object>()
            {
                { "user_type", $"{userType}" }
            };
        }
    }
}
