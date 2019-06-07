using SQLite;
using SQLiteNetExtensions.Attributes;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Objects
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(typeof(Employee))]
        public long EmployeeId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public UsersRoles Role { get; set; } = UsersRoles.Candidate;
    }
}