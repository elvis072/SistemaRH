using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using SistemaRH.Utilities;
using SistemaRH.Objects;
using Newtonsoft.Json;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Popups
{
    public class PopupJob : DialogFragment, View.IOnClickListener
    {
        private TextInputLayout tilPopupJobName;
        private TextInputLayout tilPopupJobMinSalary;
        private TextInputLayout tilPopupJobMaxSalary;
        private TextInputEditText tietPopupJobName;
        private TextInputEditText tietPopupJobMinSalary;
        private TextInputEditText tietPopupJobMaxSalary;
        private Spinner spPopupJobRiskLevel;
        private Button btnPopupJobSave;

        //Arguments
        private Job job;
        private ManagementPopupAction managementPopupAction;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Arguments != null)
            {
                int popupAction = Arguments.GetInt("popupAction", (int)ManagementPopupAction.Show);
                managementPopupAction = (ManagementPopupAction) popupAction;

                string data = Arguments.GetString("job");
                if (!string.IsNullOrEmpty(data))
                    job = JsonConvert.DeserializeObject<Job>(data);        
            }
                     
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PopupJob, container, false);
            tilPopupJobName = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupJobName);
            tilPopupJobMinSalary = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupJobMinSalary);
            tilPopupJobMaxSalary = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupJobMaxSalary);
            tietPopupJobName = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupJobName);
            tietPopupJobMinSalary = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupJobMinSalary);
            tietPopupJobMaxSalary = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupJobMaxSalary);
            spPopupJobRiskLevel = view.FindViewById<Spinner>(Resource.Id.spPopupJobRiskLevel);
            btnPopupJobSave = view.FindViewById<Button>(Resource.Id.btnPopupJobSave);

            btnPopupJobSave.SetOnClickListener(this);

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupJobName);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupJobMinSalary);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupJobMaxSalary);

            var riskLevelItems = Android.App.Application.Context.Resources.GetStringArray(Resource.Array.riskLevels);
            var risklevelAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, riskLevelItems);
            risklevelAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spPopupJobRiskLevel.Adapter = risklevelAdapter;

            switch (managementPopupAction)
            {
                case ManagementPopupAction.Show:
                    tietPopupJobName.Enabled = tietPopupJobMinSalary.Enabled = tietPopupJobMaxSalary.Enabled = spPopupJobRiskLevel.Enabled = false;
                    tilPopupJobName.Enabled = tilPopupJobMinSalary.Enabled = tilPopupJobMaxSalary.Enabled = false;
                    btnPopupJobSave.Visibility = ViewStates.Gone;

                    if (job != null)
                    {
                        tietPopupJobName.Text = job.Name;
                        spPopupJobRiskLevel.SetSelection((int)job.RiskLevel);
                        tietPopupJobMinSalary.Text = job.MinSalary.ToString();
                        tietPopupJobMaxSalary.Text = job.MaxSalary.ToString();
                    }
                    break;
                case ManagementPopupAction.Edit:
                    if (job != null)
                    {
                        tietPopupJobName.Text = job.Name;
                        spPopupJobRiskLevel.SetSelection((int)job.RiskLevel);
                        tietPopupJobMinSalary.Text = job.MinSalary.ToString();
                        tietPopupJobMaxSalary.Text = job.MaxSalary.ToString();
                    }
                    break;
            }

            return view;           
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnPopupJobSave:
                    if (Validations())
                    {          
                        switch(managementPopupAction)
                        {                       
                            case ManagementPopupAction.Create:
                                Job newJob = new Job()
                                {
                                    Name = tietPopupJobName.Text,
                                    RiskLevel = (RiskLevel)spPopupJobRiskLevel.SelectedItemPosition + 1,
                                    MinSalary = int.Parse(tietPopupJobMinSalary.Text),
                                    MaxSalary = int.Parse(tietPopupJobMaxSalary.Text),
                                    State = true                                   
                                };
                                bool isInserted = await MyLib.Instance.InsertObjectAsync(newJob);
                                if (isInserted)
                                {
                                    if (Activity != null)
                                        Toast.MakeText(Activity, Resource.String.jobCreated, ToastLength.Short).Show();
                                    Dismiss();
                                }
                                else if (Activity != null)
                                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                                break;
                            case ManagementPopupAction.Edit:
                                if (job != null)
                                {
                                    job.Name = tietPopupJobName.Text;
                                    job.RiskLevel = (RiskLevel)spPopupJobRiskLevel.SelectedItemPosition;
                                    job.MinSalary = int.Parse(tietPopupJobMinSalary.Text);
                                    job.MaxSalary = int.Parse(tietPopupJobMaxSalary.Text);
                                    bool isUpdated = await MyLib.Instance.UpdateObjectAsync(job);
                                    if (isUpdated)
                                    {
                                        if (Activity != null)
                                            Toast.MakeText(Activity, Resource.String.jobUpdated, ToastLength.Short).Show();
                                        Dismiss();
                                    }
                                    else if (Activity != null)
                                        Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                                }
                                break;
                        }
                    }
                    break;
            }    
        }

        private bool Validations()
        {
            bool valid = true;

            //Name's validations
            if (string.IsNullOrEmpty(tietPopupJobName.Text))
            {
                valid = false;
                tilPopupJobName.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            //Risk level's validations
            if (spPopupJobRiskLevel.SelectedItem?.Equals(MyLib.Instance.GetString(Resource.String.none)) ?? true)
            {
                valid = false;
                if (Activity != null)
                    Toast.MakeText(Activity, Resource.String.spinnerSelectItemError, ToastLength.Short).Show();
            }

            //Min Salary's validations
            if (string.IsNullOrEmpty(tietPopupJobMinSalary.Text))
            {
                valid = false;
                tilPopupJobMinSalary.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            //Max Salary's validations
            if (string.IsNullOrEmpty(tietPopupJobMaxSalary.Text))
            {
                valid = false;
                tilPopupJobMaxSalary.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            return valid;
        }
    }
}