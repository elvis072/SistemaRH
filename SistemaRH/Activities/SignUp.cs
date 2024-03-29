﻿using System;
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
        private TextInputLayout tilSignUpName;
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

            tilSignUpName = FindViewById<TextInputLayout>(Resource.Id.tilSignUpName);
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

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilSignUpName);
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

            //Name's validations
            if (string.IsNullOrEmpty(tietSignUpName.Text))
            {
                valid = false;
                tilSignUpName.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

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
            valid = MyLib.Instance.ValidateConfirmPassword(tilSignUpConfirmPassword, tietSignUpPassword.Text);

            //Async's validations
            if (isValidUsername)
            {
                var result = await MyLib.Instance.FindObjectsWithCustomQueryAsync<User>(
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
            bool areAllUserChildrenTablesCreated = await MyLib.Instance.CreateTables(new Type[] { typeof(Candidate), typeof(Employee) });
            if (areAllUserChildrenTablesCreated)
            {
                User newUser = new User()
                {
                    Username = tietSignUpUsername.Text,
                    Password = MyLib.Instance.EncryptText(tietSignUpPassword.Text)
                };

                bool isUserCreated = await MyLib.Instance.InsertObjectAsync(newUser);
                if (isUserCreated)
                {
                    bool areAllCandidateChildrenTablesCreated = await MyLib.Instance.CreateTables(new Type[] { typeof(Job), typeof(Department), typeof(Competition), typeof(Training), typeof(WorkExperience) });
                    if (areAllCandidateChildrenTablesCreated)
                    {
                        Candidate newCandidate = new Candidate()
                        {
                            Name = tietSignUpName.Text,
                            IdentificationCard = long.Parse(tietSignUpIdentCard.Text),
                            User = newUser
                        };

                        bool isCandidateCreated = await MyLib.Instance.InsertObjectAsync(newCandidate);
                        if (isCandidateCreated)
                        {
                            MyLib.Instance.SaveUserId(newUser.Id);
                            StartActivity(new Intent(this, typeof(CandidateJob)));
                            Finish();
                            return;
                        }
                    }
                }
            }
            Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
        }
    }
}