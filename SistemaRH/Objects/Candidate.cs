using System.Collections.Generic;
using SQLite;
using static SistemaRH.Enumerators.GlobalEnums;
using SQLiteNetExtensions.Attributes;

namespace SistemaRH.Objects
{
    public class Candidate
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public long IdentificationCard { get; set; }
        public int ExpectedSalary { get; set; }    
        public string Password { get; set; }       
        [OneToOne]         
        public Job ExpectedJob { get; set; }
        [OneToOne]
        public Department Department { get; set; }
        [OneToMany]
        public IList<Competition> Competitions { get; set; }
        [OneToMany]
        public IList<Training> Trainings { get; set; }
        [OneToMany]
        public IList<WorkExperience> WorkExperiences { get; set; }
        public string RecommendatedBy { get; set; }
        public UsersRoles Role { get; set; } = UsersRoles.Candidate;
    }
}