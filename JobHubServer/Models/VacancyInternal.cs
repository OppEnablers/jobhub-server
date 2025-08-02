using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public class VacancyInternal
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = "";

        [FirestoreProperty("employer_id")]
        public string EmployerId { get; set; } = "";

        [FirestoreProperty("applicants")]
        public List<string> Applicants { get; set; } = [];

        [FirestoreProperty("name")]
        public string Name { get; set; } = "";

        [FirestoreProperty("location")]
        public string Location { get; set; } = "";

        [FirestoreProperty("time")]
        public int Time { get; set; }

        [FirestoreProperty("shift")]
        public int Shift { get; set; }

        [FirestoreProperty("modality")]
        public int Modality { get; set; }

        [FirestoreProperty("objectives")]
        public string Objectives { get; set; } = "";

        [FirestoreProperty("skills")]
        public string Skills { get; set; } = "";

        [FirestoreProperty("minimum_salary")]
        public float MinimumSalary { get; set; }

        [FirestoreProperty("maximum_salary")]
        public float MaximumSalary { get; set; }

        [FirestoreProperty("work_experience")]
        public int WorkExperience { get; set; }

        public static VacancyInternal FromVacancy(Vacancy vacancy)
        {
            VacancyInternal vacancyInternal = new()
            {
                Id = vacancy.Id,
                Name = vacancy.Name,
                Location = vacancy.Location,
                Time = vacancy.Time,
                Shift = vacancy.Shift,
                Modality = vacancy.Modality,
                Objectives = vacancy.Objectives,
                Skills = vacancy.Skills,
                MinimumSalary = vacancy.MinimumSalary,
                MaximumSalary = vacancy.MaximumSalary,
                WorkExperience = vacancy.WorkExperience
            };
            return vacancyInternal;
        }
    }
}
