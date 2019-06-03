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

namespace SistemaRH.Objects
{
    public class Competition
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
    }
}