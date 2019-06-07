using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SistemaRH.Objects
{
    public class Candidate
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public long IdentificationCard { get; set; }
        public int ExpectedSalary { get; set; }
        [OneToOne]
        public User User { get; set; }
        [OneToOne]         
        public Job ExpectedJob { get; set; }
        [OneToOne]
        public Department Department { get; set; }
        [OneToMany]
        public List<Competition> Competitions { get; set; }
        [OneToMany]
        public List<Training> Trainings { get; set; }
        [OneToMany]
        public List<WorkExperience> WorkExperiences { get; set; }
        public string RecommendatedBy { get; set; }
    }
}