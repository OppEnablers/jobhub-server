using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public class Vacancy
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = "";

        [FirestoreProperty("name")]
        public string Name { get; set; } = "";

        [FirestoreProperty("location")]
        public string Location { get; set; } = "";

        public string CompanyName { get; set; } = "";

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

        public static Vacancy FromVacancyInternal(VacancyInternal vacancyInternal)
        {
            Vacancy vacancy = new()
            {
                Id = vacancyInternal.Id,
                Name = vacancyInternal.Name,
                Location = vacancyInternal.Location,
                Time = vacancyInternal.Time,
                Shift = vacancyInternal.Shift,
                Modality = vacancyInternal.Modality,
                Objectives = vacancyInternal.Objectives,
                Skills = vacancyInternal.Skills,
                MinimumSalary = vacancyInternal.MinimumSalary,
                MaximumSalary = vacancyInternal.MaximumSalary,
                WorkExperience = vacancyInternal.WorkExperience
            };
            return vacancy;
        }
    }
}
