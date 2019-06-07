using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SistemaRH.Objects
{
    public class WorkExperience
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(Candidate))]
        public long CandidateId { get; set; }

        public int Salary { get; set; }
        public string Enterprise { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }  
    }
}