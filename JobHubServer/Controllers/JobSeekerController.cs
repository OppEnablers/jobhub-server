using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobHubServer.Controllers
{
    [Authorize("JobSeekers")]
    [Route("api/jobseeker")]
    [ApiController]
    public class JobSeekerController(FirestoreDb db) : ControllerBase
    {
        private const string CollectionName = "jobseekers";

        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.DefaultInstance;

        [HttpGet("account_info")]
        public async Task<ActionResult<JobSeeker>> GetJobSeekerInfo()
        {
            string? userId = Request.Headers["user_id"];

            if (userId is null)
                return BadRequest();

            DocumentReference docRef = db.Collection(CollectionName).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            return snapshot.ConvertTo<JobSeeker>();
        }

        [HttpPost("update_info")]
        public async Task<ActionResult> Post([FromBody] JobSeeker updatedInfo)
        {
            string? userId = Request.Headers["user_id"];
            string? token = Request.Headers.Authorization.ToString()["Bearer ".Length..];

            if (userId is null || token is null)
                return BadRequest();

            FirebaseToken fbToken = await _firebaseAuth.VerifyIdTokenAsync(token);

            if (updatedInfo.Email != fbToken.Claims["email"].ToString())
            {
                await _firebaseAuth.UpdateUserAsync(new UserRecordArgs()
                {
                    Uid = fbToken.Uid,
                    Email = updatedInfo.Email
                });
            }

            CollectionReference colRef = db.Collection(CollectionName);
            await colRef.Document(userId).SetAsync(updatedInfo);

            return Ok();
        }
    }
}
