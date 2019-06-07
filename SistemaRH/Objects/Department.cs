using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SistemaRH.Objects
{
    public class Department
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(typeof(Employee))]
        public long EmployeeId { get; set; }

        public string Description { get; set; }
        public bool State { get; set; }
    }
}