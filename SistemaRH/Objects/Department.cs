using SQLite;

namespace SistemaRH.Objects
{
    public class Department
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
    }
}