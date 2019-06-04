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
using SistemaRH.Utilities;
using System.Text.RegularExpressions;
using SistemaRH.Objects;
using Android.Content.PM;
using System.Threading.Tasks;

namespace SistemaRH.Activities
{
    [Activity(Label = "SignUp", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustUnspecified)]
    public class SignUp : AppCompatActivity, View.IOnClickListener
    {
        private TextInputLayout tilSignUpUsername;
        private TextInputLayout tilSignUpIdentCard;
        private TextInputLayout tilSignUpPassword;
        private TextInputLayout tilSignUpConfirmPassword;
        private TextInputEditText tietSignUpName;
        private TextInputEditText tietSignUpUsername;
        private TextInputEditText tietSignUpIdentCard;
        private TextInputEditText tietSignUpPassword;
        private TextInputEditText tietSignUpConfirmPassword;
        private Button btnSignUpCreateAccount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);            
     
            tilSignUpUsername = FindViewById<TextInputLayout>(Resource.Id.tilSignUpUsername);
            tilSignUpIdentCard = FindViewById<TextInputLayout>(Resource.Id.tilSignUpIdentCard);
            tilSignUpPassword = FindViewById<TextInputLayout>(Resource.Id.tilSignUpPassword);
            tilSignUpConfirmPassword = FindViewById<TextInputLayout>(Resource.Id.tilSignUpConfirmPassword);
            tietSignUpName = FindViewById<TextInputEditText>(Resource.Id.tietSignUpName);
            tietSignUpUsername = FindViewById<TextInputEditText>(Resource.Id.tietSignUpUsername);
            tietSignUpIdentCard = FindViewById<TextInputEditText>(Resource.Id.tietSignUpIdentCard);
            tietSignUpPassword = FindViewById<TextInputEditText>(Resource.Id.tietSignUpPassword);
            tietSignUpConfirmPassword = FindViewById<TextInputEditText>(Resource.Id.tietSignUpConfirmPassword);
            btnSignUpCreateAccount = FindViewById<Button>(Resource.Id.btnSignUpCreateAccount);

            btnSignUpCreateAccount.SetOnClickListener(this);

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilSignUpUsername);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilSignUpIdentCard);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilSignUpPassword);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilSignUpConfirmPassword);
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnSignUpCreateAccount:
                    tilSignUpUsername.ErrorEnabled = tilSignUpIdentCard.ErrorEnabled = tilSignUpPassword.ErrorEnabled = tilSignUpConfirmPassword.ErrorEnabled = false;
                    if (await Validations())
                        CreateNewUser();
                    break;
            }     
        }

        private async Task<bool> Validations()
        {
            bool valid = true;
            bool isValidUsername = true;

            //Username's validations
            valid = isValidUsername = MyLib.Instance.ValidateUsername(tilSignUpUsername);

            //Identification card's validations
            if (string.IsNullOrEmpty(tietSignUpIdentCard.Text))
            {
                valid = false;
                tilSignUpIdentCard.Error = MyLib.Instance.GetString(Resource.String.identCardCannotBeEmpty);
            }

            //Password's validations
            valid = MyLib.Instance.ValidatePassword(tilSignUpPassword);

            //Confirm password's validations
            valid = MyLib.Instance.ValidateConfirmPassword(tilSignUpConfirmPassword);

            //Async's validations
            if (isValidUsername)
            {
                var res = await MyLib.Instance.FindAllObjectsAsync<Candidate>();

                var result = await MyLib.Instance.FindObjectsWithCustomQueryAsync<Candidate>(
                    new List<string>() { "*" }, 
                    new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", tietSignUpUsername.Text) } );
                if (result != null && result.Count > 0)
                {
                    valid = false;
                    tilSignUpUsername.Error = MyLib.Instance.GetString(Resource.String.usernameInUse);
                }
            }

            return valid;
        }

        private async void CreateNewUser()
        {           
            Candidate newUser = new Candidate()
            {
                Name = tietSignUpName.Text,
                Username = tietSignUpUsername.Text,
                IdentificationCard = long.Parse(tietSignUpIdentCard.Text),
                Password = MyLib.Instance.EncryptText(tietSignUpPassword.Text)             
            };

            bool areAllCandidateChildrenTablesCreated = await MyLib.Instance.CreateTables(new Type[] { typeof(Job), typeof(Department), typeof(Competition), typeof(Training), typeof(WorkExperience) });
            if (areAllCandidateChildrenTablesCreated)
            {
                bool isUserCreated = await MyLib.Instance.InsertObjectAsync(newUser);
                if (isUserCreated)
                {
                    MyLib.Instance.SaveUserId(newUser.Id);
                    StartActivity(new Intent(this, typeof(CandidateJob)));
                    Finish();
                }
                else
                    Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
            }
            else
                Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
        }
    }
}