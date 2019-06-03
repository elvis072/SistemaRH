using SQLite;

namespace SistemaRH.Objects
{
    public class Language
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}