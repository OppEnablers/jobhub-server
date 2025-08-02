using Google.Cloud.Firestore;

namespace JobHubServer.Models
{
    [FirestoreData]
    public class Job
    {
        public string Id { get; set; } = "";

        public string EmployerId { get; set; } = "";

        public string Name { get; set; } = "";

        public string Location { get; set; } = "";

        public string CompanyName { get; set; } = "";

        public int Time { get; set; }

        public int Shift { get; set; }

        public int Modality { get; set; }

        public string Objectives { get; set; } = "";

        public string Skills { get; set; } = "";

        public float MinimumSalary { get; set; }

        public float MaximumSalary { get; set; }

        public int WorkExperience { get; set; }

        public static Job FromVacancyInternal(VacancyInternal vacancyInternal)
        {
            Job job = new()
            {
                Id = vacancyInternal.Id,
                EmployerId = vacancyInternal.EmployerId,
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
            return job;
        }
    }
}
