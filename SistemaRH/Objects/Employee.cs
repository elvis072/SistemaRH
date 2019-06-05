using System;
using SQLite;

namespace SistemaRH.Objects
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long IdentificationCard { get; set; }
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public Department Department { get; set; }
        public Job Job { get; set; }
        public int MensualSalary { get; set; }
        public bool State { get; set; }
    }
}