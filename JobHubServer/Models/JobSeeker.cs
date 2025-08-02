using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public partial class JobSeeker : User
    {
        [FirestoreProperty("name")]
        public string Name { get; set; } = "";

        [FirestoreProperty("birthday")]
        public long Birthday { get; set; } = 0;

        [FirestoreProperty("skills")]
        public List<string> Skills { get; set; } = [];

        [FirestoreProperty("interests")]
        public List<int> Interests { get; set; } = [];

        [FirestoreProperty("objectives")]
        public string Objectives { get; set; } = "";

        [FirestoreProperty("education")]
        public string Education { get; set; } = "";

        [FirestoreProperty("work_experience")]
        public string WorkExperience { get; set; } = "";

        [FirestoreProperty("declined_jobs")]
        public List<string> DeclinedJobs { get; set; } = [];
    }
}
