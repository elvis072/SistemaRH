using System;
using System.Linq;
using Android.Icu.Util;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SistemaRH.Objects;
using SistemaRH.Utilities;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Popups
{
    public class PopupTraining : DialogFragment, View.IOnClickListener, Android.App.DatePickerDialog.IOnDateSetListener
    {
        private TextInputLayout tilPopupTrainingDescription;
        private TextInputLayout tilPopupTrainingFromDate;
        private TextInputLayout tilPopupTrainingToDate;
        private TextInputLayout tilPopupTrainingInstitution;
        private TextInputEditText tietPopupTrainingDescription;
        private TextInputEditText tietPopupTrainingFromDate;
        private TextInputEditText tietPopupTrainingToDate;
        private TextInputEditText tietPopupTrainingInstitution;
        private Spinner spPopupTrainingTrainingLevel;
        private Button btnPopupTrainingSave;
        private Android.App.DatePickerDialog toDatePicker;
        private Android.App.DatePickerDialog fromDatePicker;
        private DateTime fromDate;
        private DateTime toDate;

        //Arguments
        private ManagementPopupAction managementPopupAction;
        private Training training;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Arguments != null)
            {
                int popupAction = Arguments.GetInt("popupAction", (int)ManagementPopupAction.Show);
                managementPopupAction = (ManagementPopupAction)popupAction;

                string data = Arguments.GetString("training");
                if (!string.IsNullOrEmpty(data))
                    training = JsonConvert.DeserializeObject<Training>(data);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PopupTraining, container, false);
            tilPopupTrainingDescription = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupTrainingDescription);
            tilPopupTrainingFromDate = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupTrainingFromDate);
            tilPopupTrainingToDate = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupTrainingToDate);
            tilPopupTrainingInstitution = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupTrainingInstitution);
            tietPopupTrainingDescription = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupTrainingDescription);
            tietPopupTrainingFromDate = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupTrainingFromDate);
            tietPopupTrainingToDate = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupTrainingToDate);
            tietPopupTrainingInstitution = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupTrainingInstitution);
            spPopupTrainingTrainingLevel = view.FindViewById<Spinner>(Resource.Id.spPopupTrainingTrainingLevel);
            btnPopupTrainingSave = view.FindViewById<Button>(Resource.Id.btnPopupTrainingSave);

            var trainingLevelsItems = Android.App.Application.Context.Resources.GetStringArray(Resource.Array.trainingLevels).ToList();
            var traininglevelsAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, trainingLevelsItems);
            traininglevelsAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spPopupTrainingTrainingLevel.Adapter = traininglevelsAdapter;

            Calendar calendar = Calendar.Instance;
            fromDatePicker = new Android.App.DatePickerDialog(Activity, Resource.Style.MyDatePickerStyle, this, calendar.Get(CalendarField.Year),
                calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));
            toDatePicker = new Android.App.DatePickerDialog(Activity, Resource.Style.MyDatePickerStyle, this, calendar.Get(CalendarField.Year),
                calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth));

            switch (managementPopupAction)
            {
                case ManagementPopupAction.Show:
                    tietPopupTrainingFromDate.Enabled = tietPopupTrainingToDate.Enabled = tietPopupTrainingDescription.Enabled = tietPopupTrainingInstitution.Enabled = false;
                    tilPopupTrainingFromDate.Enabled = tilPopupTrainingToDate.Enabled = tilPopupTrainingDescription.Enabled = tilPopupTrainingInstitution.Enabled = false;
                    spPopupTrainingTrainingLevel.Enabled = false;
                    btnPopupTrainingSave.Visibility = ViewStates.Gone;
                    break;
                case ManagementPopupAction.Edit:
                    if (training != null)
                    {
                        tietPopupTrainingDescription.Text = training.Description;
                        tietPopupTrainingFromDate.Text = training.FromDate.ToShortDateString();
                        tietPopupTrainingToDate.Text = training.ToDate.ToShortDateString();
                        tietPopupTrainingInstitution.Text = training.Institution;
                        spPopupTrainingTrainingLevel.SetSelection((int)training.TrainingLevel);

                        fromDatePicker = new Android.App.DatePickerDialog(Activity, Resource.Style.MyDatePickerStyle, this, training.FromDate.Year,
                            training.FromDate.Month, training.FromDate.Day);
                        toDatePicker = new Android.App.DatePickerDialog(Activity, Resource.Style.MyDatePickerStyle, this, training.ToDate.Year,
                            training.ToDate.Month, training.ToDate.Day);
                    }
                    break;
                case ManagementPopupAction.Create:
                    trainingLevelsItems.Insert(0, MyLib.Instance.GetString(Resource.String.none));
                    traininglevelsAdapter.NotifyDataSetChanged();
                    break;
            }

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupTrainingDescription);
            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupTrainingInstitution);

            tietPopupTrainingFromDate.SetOnClickListener(this);
            tietPopupTrainingToDate.SetOnClickListener(this);
            btnPopupTrainingSave.SetOnClickListener(this);

            return view;
        }

        public async void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.tietPopupTrainingFromDate:
                    fromDatePicker?.Show();
                    break;
                case Resource.Id.tietPopupTrainingToDate:
                    toDatePicker?.Show();
                    break;
                case Resource.Id.btnPopupTrainingSave:
                    if (Validations())
                    {
                        switch(managementPopupAction)
                        {
                            case ManagementPopupAction.Create:
                                Training newTraining = new Training()
                                {
                                    Description = tietPopupTrainingDescription.Text,
                                    FromDate = fromDate,
                                    ToDate = toDate,
                                    TrainingLevel = (TrainingLevel) spPopupTrainingTrainingLevel.SelectedItemPosition + 1,
                                    Institution = tietPopupTrainingInstitution.Text
                                };
                                bool isInserted = await MyLib.Instance.InsertObjectAsync(newTraining);
                                if (isInserted)
                                {
                                    if (Activity != null)
                                        Toast.MakeText(Activity, Resource.String.trainingCreated, ToastLength.Short).Show();
                                    Dismiss();
                                }
                                else if (Activity != null)
                                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                                break;
                            case ManagementPopupAction.Edit:
                                if (training != null)
                                {
                                    training.Description = tietPopupTrainingDescription.Text;
                                    training.FromDate = fromDate;
                                    training.ToDate = toDate;
                                    training.TrainingLevel = (TrainingLevel)spPopupTrainingTrainingLevel.SelectedItemPosition;
                                    training.Institution = tietPopupTrainingInstitution.Text;
                                    bool isUpdated = await MyLib.Instance.UpdateObjectAsync(training);
                                    if (isUpdated)
                                    {
                                        if (Activity != null)
                                            Toast.MakeText(Activity, Resource.String.trainingUpdated, ToastLength.Short).Show();
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
            if (string.IsNullOrEmpty(tietPopupTrainingDescription.Text))
            {
                valid = false;
                tilPopupTrainingDescription.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietPopupTrainingFromDate.Text))
            {
                valid = false;
                tilPopupTrainingFromDate.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietPopupTrainingToDate.Text))
            {
                valid = false;
                tilPopupTrainingToDate.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }

            if (string.IsNullOrEmpty(tietPopupTrainingInstitution.Text))
            {
                valid = false;
                tilPopupTrainingInstitution.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }
            return valid;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (view == fromDatePicker.DatePicker)
            {
                fromDate = new DateTime(year, month, dayOfMonth);
                tietPopupTrainingFromDate.Text = fromDate.ToShortDateString();
            }
            else if (view == toDatePicker.DatePicker)
            {
                toDate = new DateTime(year, month, dayOfMonth);
                tietPopupTrainingToDate.Text = toDate.ToShortDateString();
            }
        }
    }
}