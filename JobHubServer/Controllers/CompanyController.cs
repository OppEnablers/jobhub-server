using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController(FirestoreDb db) : ControllerBase
    {
        private const string CollectionName = "companies";

        [HttpGet("get")]
        public async Task<IEnumerable<Company>> GetCompanies()
        {            
            var colRef = db.Collection(CollectionName);
            var snapshot = await colRef.GetSnapshotAsync();

            var companies = snapshot.Documents.Select(d => d.ConvertTo<Company>()).ToList();

            return companies;
        }

        [Authorize] 
        [HttpPost("add")]
        public async Task AddCompany(Company company)
        {
            DocumentReference docRef = db.Collection(CollectionName).Document();
            await docRef.SetAsync(company);
        }
    }
}
