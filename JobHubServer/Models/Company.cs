using Google.Cloud.Firestore;

namespace JobHubServer.Models
{

    [FirestoreData]
    public class Company
    {
        [FirestoreProperty("name")]
        public string Name { get; set; } = "";

        [FirestoreProperty("address")]
        public string Address { get; set; } = "";
    }
}
