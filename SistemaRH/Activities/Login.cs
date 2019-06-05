using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using Android.Content;
using SistemaRH.Utilities;
using SistemaRH.Objects;
using System.Collections.Generic;
using System.Linq;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Activities
{
    [Activity(Label = "SistemaRH", MainLauncher = true, Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustUnspecified)]
    public class Login : AppCompatActivity, View.IOnClickListener
    {
        TextInputLayout tilLoginUsername, tilLoginPassword;
        TextInputEditText tietLoginUsername, tietLoginPassword;
        Button btnLogin;
        TextView tvLoginSignUp;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);        
            SetContentView(Resource.Layout.Login);

            //await MyLib.Instance.DeleteAllObjectsAsync<Job>();
            //await MyLib.Instance.DeleteAllObjectsAsync<Department>();
            //await MyLib.Instance.DeleteAllObjectsAsync<Competition>();
            //await MyLib.Instance.DeleteAllObjectsAsync<Training>();
            //await SampleData.Instance.CreateJobs();
            //await SampleData.Instance.CreateDepartments();
            //await SampleData.Instance.CreateCompetitions();
            //await SampleData.Instance.CreateTrainings();
            //StartActivity(new Intent(this, typeof(CandidateJob)));

            //User is logged
            if (MyLib.Instance.GetUserId() != 0)
                MyLib.Instance.OpenMainActivity(this);

            tilLoginUsername = FindViewById<TextInputLayout>(Resource.Id.tilLoginUsername);
            tilLoginPassword = FindViewById<TextInputLayout>(Resource.Id.tilLoginPassword);
            tietLoginUsername = FindViewById<TextInputEditText>(Resource.Id.tietLoginUsername);
            tietLoginPassword = FindViewById<TextInputEditText>(Resource.Id.tietLoginPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            tvLoginSignUp = FindViewById<TextView>(Resource.Id.tvLoginSignUp);

            btnLogin.SetOnClickListener(this);
            tvLoginSignUp.SetOnClickListener(this);

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilLoginUsername);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilLoginPassword);          
        }

        public void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnLogin:
                    if (Validations())
                    {
                        tilLoginUsername.ErrorEnabled = tilLoginPassword.ErrorEnabled = false;
                        ExecuteLogin();
                    }
                    break;
                case Resource.Id.tvLoginSignUp:
                    StartActivity(new Intent(this, typeof(SignUp)));
                    break;
            }   
        }

        private bool Validations()
        {
            bool valid = true;

            //Username's validations                  
            valid = MyLib.Instance.ValidateUsername(tilLoginUsername);

            //Password's validations
            valid = MyLib.Instance.ValidatePassword(tilLoginPassword);     

            return valid;
        }

        private async void ExecuteLogin()
        {
            Candidate user = new Candidate()
            {
                Username = tietLoginUsername.Text,
                Password = MyLib.Instance.EncryptText(tietLoginPassword.Text)
            };

            var result = await MyLib.Instance.FindObjectsWithCustomQueryAsync<Candidate>(
                new List<string>() { "*" },
                new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>(nameof(user.Username), user.Username),
                    new KeyValuePair<string, string>(nameof(user.Password), user.Password)});

            if (result != null && result.Count > 0)
            {            
                user = await MyLib.Instance.FindObjectAsync<Candidate>(result.FirstOrDefault().Id);
                MyLib.Instance.SaveUserId(user.Id);
                if (user.Role == UsersRoles.Candidate)
                {                  
                    //Expected job, department or expected salary are not selected
                    if (user.ExpectedJob == null || user.Department == null || user.ExpectedSalary == default(int))
                        StartActivity(new Intent(this, typeof(CandidateJob)));
                    //Competitions or trainings are not selected
                    else if (user.Competitions == null || user.Trainings == null)
                        StartActivity(new Intent(this, typeof(CandidateSkills)));
                    //Work experiences is not filled         
                    else if (user.WorkExperiences == null)
                        StartActivity(new Intent(this, typeof(CandidateExperience)));
                    else //If all is filled, go to Home                        
                        MyLib.Instance.OpenMainActivity(this);
                    this?.Finish();
                }
                else
                    MyLib.Instance.OpenMainActivity(this);
            }
            else
                Toast.MakeText(this, Resource.String.invalidUsernameOrPassword, ToastLength.Short).Show();
        }
    }
}

