using System;
using SQLite;
using SQLiteNetExtensions.Attributes;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Objects
{
    public class Training
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(Candidate))]
        public long CandidateId { get; set; }

        public string Description { get; set; }
        public TrainingLevel TrainingLevel { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Institution { get; set; }
    }
}