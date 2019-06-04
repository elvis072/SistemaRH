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

        public string Description { get; set; }
        public bool State { get; set; }
    }
}