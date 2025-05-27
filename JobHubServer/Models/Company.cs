using Google.Cloud.Firestore;

namespace JobHubServer.Models
{

    [FirestoreData]
    public class Company
    {
        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string address { get; set; }
    }
}
