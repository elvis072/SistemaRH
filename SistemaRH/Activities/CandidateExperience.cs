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
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using SistemaRH.Utilities;
using SistemaRH.Objects;
using Android.Icu.Util;
using Android.Graphics;
using System.Threading.Tasks;

namespace SistemaRH.Activities
{
    [Activity(Label = "CandidateExperience", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CandidateExperience : AppCompatActivity, View.IOnClickListener, DatePickerDialog.IOnDateSetListener
    {
        private TextInputLayout tilCandidateExperienceSalary;
        private TextInputLayout tilCandidateExperienceEnterprise;
        private TextInputLayout tilCandidateExperienceFromDate;
        private TextInputLayout tilCandidateExperienceToDate;
        private TextInputEditText tietCandidateExperienceSalary;
        private TextInputEditText tietCandidateExperienceEnterprise;
        private TextInputEditText tietCandidateExperienceFromDate;
        private TextInputEditText tietCandidateExperienceToDate;
        private TextInputEditText tietCandidateExperienceRecommendations;
        private Button btnCandidateExperienceNext;
        private Button btnCandidateExperienceAddWorkExperience;
        private DatePickerDialog toDatePicker;
        private DatePickerDialog fromDatePicker;
        private DateTime fromDate;
        private DateTime toDate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CandidateExperience);

            tilCandidateExperienceSalary = FindViewById<TextInputLayout>(Resource.Id.tilCandidateExperienceSalary);
            tilCandidateExperienceEnterprise = FindViewById<TextInputLayout>(Resource.Id.tilCandidateExperienceEnterprise);
            tilCandidateExperienceFromDate = FindViewById<TextInputLayout>(Resource.Id.tilCandidateExperienceFromDate);
            tilCandidateExperienceToDate = FindViewById<TextInputLayout>(Resource.Id.tilCandidateExperienceToDate);
            tietCandidateExperienceSalary = FindViewById<TextInputEditText>(Resource.Id.tietCandidateExperienceSalary);
            tietCandidateExperienceEnterprise = FindViewById<TextInputEditText>(Resource.Id.tietCandidateExperienceEnterprise);
            tietCandidateExperienceFromDate = FindViewById<TextInputEditText>(Resource.Id.tietCandidateExperienceFromDate);
            tietCandidateExperienceToDate = FindViewById<TextInputEditText>(Resource.Id.tietCandidateExperienceToDate);
            tietCandidateExperienceRecommendations = FindViewById<TextInputEditText>(Resource.Id.tietCandidateExperienceRecommendations);
            btnCandidateExperienceNext = FindViewById<Button>(Resource.Id.btnCandidateExperienceNext);
            btnCandidateExperienceAddWorkExperience = FindViewById<Button>(Resource.Id.btnCandidateExperienceAddWorkExperience);

            btnCandidateExperienceNext.SetOnClickListener(this);
            btnCandidateExperienceAddWorkExperience.SetOnClickListener(this);
            tietCandidateExperienceFromDate.SetOnClickListener(this);
            tietCandidateExperienceToDate.SetOnClickListener(this);

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilCandidateExperienceSalary);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilCandidateExperienceEnterprise);

            Calendar calendar = Calendar.Instance;           
            fromDatePicker = new DatePickerDialog(this, Resource.Style.MyDatePickerStyle, this, calendar.Get(CalendarField.Year), 
                calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));
            toDatePicker = new DatePickerDialog(this, Resource.Style.MyDatePickerStyle, this, calendar.Get(CalendarField.Year),
           calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.tietCandidateExperienceFromDate:
                    fromDatePicker.Show();
                    break;
                case Resource.Id.tietCandidateExperienceToDate:
                    toDatePicker.Show();
                    break;
                case Resource.Id.btnCandidateExperienceAddWorkExperience:
                    SaveWorkExperience().GetAwaiter();
                    break;
                case Resource.Id.btnCandidateExperienceNext:
                    if (await SaveWorkExperience())
                    {
                        StartActivity(new Intent(this, typeof(Main)));
                        Finish();
                    }
                    else
                        Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
                    break;
            }       
        }

        private bool Validations()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(tietCandidateExperienceSalary.Text))
            {
                valid = false;
                tilCandidateExperienceSalary.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietCandidateExperienceEnterprise.Text))
            {
                valid = false;
                tilCandidateExperienceEnterprise.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietCandidateExperienceFromDate.Text))
            {
                valid = false;
                tilCandidateExperienceFromDate.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietCandidateExperienceToDate.Text))
            {
                valid = false;
                tilCandidateExperienceToDate.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }
            return valid;
        }

        private async Task<bool> SaveWorkExperience()
        {
            bool isSucessfull = false;
            if (Validations())
            {
                WorkExperience workExperience = new WorkExperience()
                {
                    Salary = int.Parse(tietCandidateExperienceSalary.Text),
                    Enterprise = tietCandidateExperienceEnterprise.Text,
                    FromDate = fromDate,
                    ToDate = toDate
                };

                bool isInserted = await MyLib.Instance.InsertObjectAsync(workExperience);
                if (isInserted)
                {
                    var user = await MyLib.Instance.FindObjectAsync<Candidate>(MyLib.Instance.GetUserId());
                    if (user != null)
                    {
                        if (user.WorkExperiences == null)
                            user.WorkExperiences = new List<WorkExperience>();
                        user.WorkExperiences.Add(workExperience);
                        bool isUpdated = await MyLib.Instance.UpdateObjectAsync(user);
                        if (isUpdated)
                        {   
                            //Reset values
                            tietCandidateExperienceSalary.Text = tietCandidateExperienceEnterprise.Text = tietCandidateExperienceFromDate.Text =
                                tietCandidateExperienceToDate.Text = tietCandidateExperienceRecommendations.Text = string.Empty;
                            Calendar calendar = Calendar.Instance;
                            fromDatePicker.UpdateDate(calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));
                            toDatePicker.UpdateDate(calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));

                            //Show confirmation message
                            Toast.MakeText(this, Resource.String.workExperienceAdded, ToastLength.Short).Show();
                            isSucessfull = true;
                        }
                        else
                            Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
            }
            return isSucessfull;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (view == fromDatePicker.DatePicker)
            {
                fromDate = new DateTime(year, month, dayOfMonth);
                tietCandidateExperienceFromDate.Text = fromDate.ToShortDateString();
            }
            else if (view == toDatePicker.DatePicker)
            {
                toDate = new DateTime(year, month, dayOfMonth);
                tietCandidateExperienceToDate.Text = toDate.ToShortDateString();
            }
        }
    }
}