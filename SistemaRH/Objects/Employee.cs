using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SistemaRH.Objects
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long IdentificationCard { get; set; }
        public string Name { get; set; }
        [OneToOne]
        public User User { get; set; }
        [OneToOne]
        public Department Department { get; set; }
        [OneToOne]
        public Job Job { get; set; }
        public DateTime EntryDate { get; set; }
        public int MensualSalary { get; set; }
        public bool State { get; set; }
    }
}