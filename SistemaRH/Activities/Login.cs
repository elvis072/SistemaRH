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
using System.Threading.Tasks;

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

            //if (MyLib.Instance.GetUserId() == 0)
            //{
            //    await MyLib.Instance.DeleteAllObjectsAsync<Job>();
            //    await MyLib.Instance.DeleteAllObjectsAsync<Department>();
            //    await MyLib.Instance.DeleteAllObjectsAsync<Competition>();
            //    await MyLib.Instance.DeleteAllObjectsAsync<Training>();
            //    await SampleData.Instance.CreateJobs();
            //    await SampleData.Instance.CreateDepartments();
            //    await SampleData.Instance.CreateCompetitions();
            //    await SampleData.Instance.CreateTrainings();
            //}

            //    await SampleData.Instance.CreateJobs();
            //    await SampleData.Instance.CreateDepartments();
            //    await SampleData.Instance.CreateCompetitions();
            //    await SampleData.Instance.CreateTrainings();

            //User is logged
            if (MyLib.Instance.GetUserId() != 0)
            {
                var user = await MyLib.Instance.FindObjectAsync<User>(MyLib.Instance.GetUserId());
                bool result = await CheckIfAllCandidateDataIsFilled(user);
                if (result)
                    return;
            }

            SetContentView(Resource.Layout.Login);
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
            User user = new User()
            {
                Username = tietLoginUsername.Text,
                Password = MyLib.Instance.EncryptText(tietLoginPassword.Text)
            };

            var result = await MyLib.Instance.FindObjectsWithCustomQueryAsync<User>(
                new List<string>() { "*" },
                new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>(nameof(user.Username), user.Username),
                    new KeyValuePair<string, string>(nameof(user.Password), user.Password)});

            if (result != null && result.Count > 0)
            {
                user = result.FirstOrDefault();
                MyLib.Instance.SaveUserId(user.Id);
                CheckIfAllCandidateDataIsFilled(user).GetAwaiter();
            }
            else
                Toast.MakeText(this, Resource.String.invalidUsernameOrPassword, ToastLength.Short).Show();
        }

        private async Task<bool> CheckIfAllCandidateDataIsFilled(User user)
        {
            if (user != null)
            {
                if (user.Role == UsersRoles.Candidate)
                {
                    Candidate candidate = await MyLib.Instance.FindObjectAsync<Candidate>(user.CandidateId);
                    if (candidate != null)
                    {
                        //Expected job, department or expected salary are not selected
                        if (candidate.ExpectedJob == null || candidate.Department == null || candidate.ExpectedSalary == default(int))
                            StartActivity(new Intent(this, typeof(CandidateJob)));
                        //Competitions or trainings are not selected
                        else if (candidate.Competitions == null || candidate.Competitions.Count == 0 || candidate.Trainings == null || candidate.Trainings.Count == 0)
                            StartActivity(new Intent(this, typeof(CandidateSkills)));
                        //Work experiences is not filled         
                        else if (candidate.WorkExperiences == null || candidate.WorkExperiences.Count == 0)
                            StartActivity(new Intent(this, typeof(CandidateExperience)));
                        else //If all is filled, go to Home                        
                            MyLib.Instance.OpenMainActivity(this);
                        this?.Finish();
                        return true;
                    }
                    else
                        Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
                }
                else
                    MyLib.Instance.OpenMainActivity(this);
            }
            else
                Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
            return false;
        }
    }
}

