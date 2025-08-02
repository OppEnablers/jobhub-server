using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private const string JobSeekersCollection = "jobseekers";
        private const string EmployersCollection = "employers";
        private const string VacanciesCollection = "vacancies";

        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.DefaultInstance;

        [HttpGet("account_info")]
        public async Task<ActionResult<JobSeeker>> GetAccountInfo()
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? _);
            if (userId is null) return BadRequest();

            DocumentReference docRef = db.Collection(JobSeekersCollection).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            return snapshot.ConvertTo<JobSeeker>();
        }

        [HttpPost("update_info")]
        public async Task<ActionResult> UpdateInfo([FromBody] JobSeeker updatedInfo)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);
            if (userId is null || token is null) return BadRequest();

            FirebaseToken fbToken = await _firebaseAuth.VerifyIdTokenAsync(token);

            if (updatedInfo.Email != fbToken.Claims["email"].ToString())
            {
                await _firebaseAuth.UpdateUserAsync(new UserRecordArgs()
                {
                    Uid = fbToken.Uid,
                    Email = updatedInfo.Email
                });
            }

            CollectionReference colRef = db.Collection(JobSeekersCollection);
            await colRef.Document(userId).SetAsync(updatedInfo);

            return Ok();
        }

        [HttpGet("jobs")]
        public async Task<ActionResult<Job[]>> GetJobs()
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);
            if (userId is null || token is null) return BadRequest();

            CollectionReference colRef = db.Collection(VacanciesCollection);
            QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

            CollectionReference empColRef = db.Collection(EmployersCollection);
            QuerySnapshot empColSnapshot = await empColRef.GetSnapshotAsync();

            JobSeeker? accountInfo = (await GetAccountInfo()).Value;
            if (accountInfo is null) return Problem();

            /*
             * filtering/algorithm goes here
             * 
             * for now, just show all available jobs since we don't have enough
             * sample data
             */
            Job[] result = [.. snapshot.Select(d => d.ConvertTo<VacancyInternal>())
                .Where(d => !d.Applicants.Contains(userId) && !accountInfo.DeclinedJobs.Contains(d.Id))
                .Select(d =>
                {
                    Job job = Job.FromVacancyInternal(d);

                    DocumentSnapshot? empSnapshot = empColSnapshot.FirstOrDefault(d => d.Id == job.EmployerId);
                    if (empSnapshot is null) return job; // rip
                    
                    Employer emp = empSnapshot.ConvertTo<Employer>();
                    job.CompanyName = emp.CompanyName;
                    return job;
                })];

            return result;
        }

        [HttpPost("accept_job")]
        public async Task<ActionResult> AcceptJob([FromBody] Job job)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);
            if (userId is null || token is null) return BadRequest();

            CollectionReference colRef = db.Collection(VacanciesCollection);
            DocumentSnapshot snapshot = await colRef.Document(job.Id).GetSnapshotAsync();
            VacancyInternal vacancyInternal = snapshot.ConvertTo<VacancyInternal>();
            vacancyInternal.Applicants.Add(userId);

            await colRef.Document(job.Id).SetAsync(vacancyInternal);

            return Ok();
        }

        [HttpPost("decline_job")]
        public async Task<ActionResult> DeclineJob([FromBody] Job job)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);
            if (userId is null || token is null) return BadRequest();

            JobSeeker? accountInfo = (await GetAccountInfo()).Value;
            if (accountInfo is null) return Problem();

            accountInfo.DeclinedJobs.Add(job.Id);

            await UpdateInfo(accountInfo);

            return Ok();
        }

        [HttpGet("applications")]
        public async Task<ActionResult<Job[]>> GetApplications()
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);
            if (userId is null || token is null) return BadRequest();

            CollectionReference colRef = db.Collection(VacanciesCollection);
            QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

            CollectionReference empColRef = db.Collection(EmployersCollection);
            QuerySnapshot empColSnapshot = await empColRef.GetSnapshotAsync();

            JobSeeker? accountInfo = (await GetAccountInfo()).Value;
            if (accountInfo is null) return Problem();

            Job[] result = [.. snapshot.Select(d => d.ConvertTo<VacancyInternal>())
                .Where(d => d.Applicants.Contains(userId))
                .Select(d =>
                {
                    Job job = Job.FromVacancyInternal(d);

                    DocumentSnapshot? empSnapshot = empColSnapshot.FirstOrDefault(d => d.Id == job.EmployerId);
                    if (empSnapshot is null) return job; // rip
                    
                    Employer emp = empSnapshot.ConvertTo<Employer>();
                    job.CompanyName = emp.CompanyName;
                    return job;
                })];

            return result;
        }
    }
}
