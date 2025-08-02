using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text;

namespace JobHubServer.Controllers
{
    [Authorize("Employers")]
    [Route("api/employer")]
    [ApiController]
    public class EmployerController(FirestoreDb db) : ControllerBase
    {
        private const string EmployerCollection = "employers";
        private const string VacanciesCollection = "vacancies";

        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.DefaultInstance;

        [HttpGet("account_info")]
        public async Task<ActionResult<Employer>> GetAccountInfo()
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? _);
            if (userId is null) return BadRequest();

            DocumentReference docRef = db.Collection(EmployerCollection).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            return snapshot.ConvertTo<Employer>();
        }

        [HttpPost("update_info")]
        public async Task<ActionResult> UpdateInfo([FromBody] Employer updatedInfo)
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

            CollectionReference colRef = db.Collection(EmployerCollection);
            await colRef.Document(userId).SetAsync(updatedInfo);

            return Ok();
        }

        [HttpPost("add_vacancy")]
        public async Task<ActionResult> AddVacancy([FromBody] Vacancy vacancy)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);

            if (userId is null || token is null)
                return BadRequest();

            DocumentReference docRef = db.Collection(VacanciesCollection).Document();

            VacancyInternal vacancyInternal = VacancyInternal.FromVacancy(vacancy);

            vacancyInternal.Id = docRef.Id;
            vacancyInternal.EmployerId = userId;

            await docRef.SetAsync(vacancyInternal);

            return Ok();
        }

        [HttpPost("update_vacancy")]
        public async Task<ActionResult> UpdateVacancy([FromBody] Vacancy vacancy)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);

            CollectionReference colRef = db.Collection(VacanciesCollection);
            DocumentSnapshot snapshot = await colRef.Document(vacancy.Id).GetSnapshotAsync();
            VacancyInternal vacancyInternal = snapshot.ConvertTo<VacancyInternal>();

            VacancyInternal updated = VacancyInternal.FromVacancy(vacancy);
            updated.EmployerId = vacancyInternal.EmployerId;
            updated.Applicants = vacancyInternal.Applicants;

            await colRef.Document(vacancy.Id).SetAsync(updated);

            return Ok();
        }

        [HttpGet("vacancies")]
        public async Task<ActionResult<Vacancy[]>> GetVacancies()
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);

            CollectionReference colRef = db.Collection(VacanciesCollection);
            QuerySnapshot snapshot = await colRef.WhereEqualTo("employer_id", userId)
                .GetSnapshotAsync();

            return snapshot.Documents.Select(d => d.ConvertTo<Vacancy>()).ToArray();
        }

        [HttpGet("vacancy/{vacancyId}")]
        public async Task<ActionResult<Vacancy>> GetVacancy(string? vacancyId)
        {
            Utility.VerifyUserRequest(Request, out string? userId, out string? token);

            CollectionReference colRef = db.Collection(VacanciesCollection);
            QuerySnapshot snapshot = await colRef.WhereEqualTo("employer_id", userId)
                .GetSnapshotAsync();

            if (vacancyId is null) return BadRequest();

            Vacancy? vacancy = snapshot.Documents
                .Select(d => d.ConvertTo<Vacancy>())
                .FirstOrDefault(v => v.Id == vacancyId);

            if (vacancy is null) return NotFound();

            return vacancy;
        }
    }
}
