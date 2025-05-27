using Google.Cloud.Firestore;
using JobHubServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpGet("get")]
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            FirestoreDb firestoreDb = HttpContext.RequestServices.GetRequiredService<FirestoreDb>();

            var collection = firestoreDb.Collection("company");
            var snapshot = await collection.GetSnapshotAsync();

            var companies = snapshot.Documents.Select(d => d.ConvertTo<Company>()).ToList();

            return companies;
        }

        [HttpPost("add")]
        public async Task AddCompany(Company company)
        {

        }
    }
}
