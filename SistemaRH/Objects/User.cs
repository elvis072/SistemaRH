using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using static SistemaReclutamientoSeleccionRH.Enumerators.GlobalEnums;

namespace SistemaReclutamientoSeleccionRH.Objects
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } = (int)UsersRoles.User;
    }
}