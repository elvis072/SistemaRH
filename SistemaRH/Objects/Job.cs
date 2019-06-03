using SQLite;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Objects
{
    public class Job
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public bool State { get; set; }
    }
}