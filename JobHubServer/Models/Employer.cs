using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public partial class Employer : User
    {
        [FirestoreProperty("name")]
        public string CompanyName { get; set; } = "";
    }
}
