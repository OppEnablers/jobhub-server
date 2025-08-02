using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty("user_id")]
        public string UserId { get; set; } = "";

        [FirestoreProperty("email")]
        public string Email { get; set; } = "";

        [FirestoreProperty("phone_number")]
        public string PhoneNumber { get; set; } = "";
    }
}
