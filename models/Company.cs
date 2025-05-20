using Google.Cloud.Firestore;

namespace models {
    [FirestoreData]
    public class Company
    {
        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string address { get; set; }
    }
}