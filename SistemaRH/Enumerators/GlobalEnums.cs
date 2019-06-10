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

namespace SistemaRH.Enumerators
{
    public class GlobalEnums
    {
        public enum UsersRoles
        {
            Employee,
            Candidate
        }

        public enum RiskLevel
        {
            Low,
            Medium,
            High
        }

        public enum TrainingLevel
        {
            Grade,
            Postgraduate,
            Master,
            Doctorate,
            Technical
        }

        public enum ManagementSwipeActions
        {
            None,
            Delete,
            Add,
            DeleteAndAdd
        }

        public enum EmployeeOptions : int
        {
            CompetenctiesManagement = 0,
            LanguagesManagement = 1,
            TrainingManagement = 2,
            JobsManagement = 3
        }
    }
}