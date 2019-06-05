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
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Content.PM;
using SistemaRH.Utilities;
using SistemaRH.Objects;

namespace SistemaRH.Activities
{
    [Activity(Label = "CandidateJob", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CandidateJob : AppCompatActivity, View.IOnClickListener
    {
        private Spinner spCandidateJobJob;
        private Spinner spCandidateJobDepartment;
        private TextInputLayout tilCandidateJobExpectedSalary;
        private TextInputEditText tietCandidateJobExpectedSalary;
        private Button btnCandidateJobNext;

        private List<Job> jobs;
        private List<Department> departments;
        private ArrayAdapter<string> jobsAdapter;
        private ArrayAdapter<string> departmentsAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CandidateJob);

            spCandidateJobJob = FindViewById<Spinner>(Resource.Id.spCandidateJobJob);
            spCandidateJobDepartment = FindViewById<Spinner>(Resource.Id.spCandidateJobDepartment);
            tilCandidateJobExpectedSalary = FindViewById<TextInputLayout>(Resource.Id.tilCandidateJobExpectedSalary);
            tietCandidateJobExpectedSalary = FindViewById<TextInputEditText>(Resource.Id.tietCandidateJobExpectedSalary);
            btnCandidateJobNext = FindViewById<Button>(Resource.Id.btnCandidateJobNext);

            btnCandidateJobNext.SetOnClickListener(this);

            jobsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem);
            jobsAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCandidateJobJob.Adapter = jobsAdapter;           

            departmentsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem);
            departmentsAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCandidateJobDepartment.Adapter = departmentsAdapter;

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilCandidateJobExpectedSalary);
            GetDataForSpinner();
        }   

        private async void GetDataForSpinner()
        {
            //Jobs
            List<string> jobsNames = new List<string>() { MyLib.Instance.GetString(Resource.String.none) };
            jobs = await MyLib.Instance.FindAllObjectsAsync<Job>();
            if (jobs != null && jobs.Count > 0)
            {
                for (int i = 0; i < jobs.Count; i++)
                {
                    if (jobs[i]?.State ?? false)
                        jobsNames.Add(jobs[i].Name);
                    else
                        jobs.RemoveAt(i);
                }
                jobsAdapter.AddAll(jobsNames);
                jobsAdapter.NotifyDataSetChanged();
            }

            //Departments
            List<string> departmnetsNames = new List<string>() { MyLib.Instance.GetString(Resource.String.none) };
            departments = await MyLib.Instance.FindAllObjectsAsync<Department>();
            if (departments != null && departments.Count > 0)
            {
                for (int i = 0; i < departments.Count; i++)
                {
                    if (departments[i]?.State ?? false)
                        departmnetsNames.Add(departments[i].Description);
                    else
                        departments.RemoveAt(i);
                }
                departmentsAdapter.AddAll(departmnetsNames);
                departmentsAdapter.NotifyDataSetChanged();
            }
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnCandidateJobNext:
                    tilCandidateJobExpectedSalary.ErrorEnabled = false;
                    if (Validations())
                    {
                        if (jobs != null && jobs.Count > 0 && departments != null && departments.Count > 0)
                        {
                            var user = await MyLib.Instance.FindObjectAsync<Candidate>(MyLib.Instance.GetUserId());
                            if (user != null)
                            {
                                user.ExpectedJob = jobs[spCandidateJobJob.SelectedItemPosition - 1];
                                user.Department = departments[spCandidateJobDepartment.SelectedItemPosition - 1];
                                user.ExpectedSalary = int.Parse(tietCandidateJobExpectedSalary.Text);
                                var res = await MyLib.Instance.FindObjectAsync<Candidate>(user.Id);
                                bool isUpdated = await MyLib.Instance.UpdateObjectAsync(user);
                                if (isUpdated)
                                {
                                    StartActivity(new Intent(this, typeof(CandidateSkills)));
                                    Finish();
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
                    break;
            }     
        }

        private bool Validations()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(tietCandidateJobExpectedSalary.Text))
            {
                valid = false;
                tilCandidateJobExpectedSalary.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }
            if ((spCandidateJobJob.SelectedItem?.Equals(MyLib.Instance.GetString(Resource.String.none)) ?? true) ||
                (spCandidateJobDepartment.SelectedItem?.Equals(MyLib.Instance.GetString(Resource.String.none)) ?? true))
            {               
                valid = false;
                Toast.MakeText(this, Resource.String.spinnerSelectItemError, ToastLength.Short).Show();
            }            
            return valid;
        }
    }
}