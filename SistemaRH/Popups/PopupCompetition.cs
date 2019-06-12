using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
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
    public class PopupCompetition : DialogFragment, View.IOnClickListener
    {
        private TextInputLayout tilPopupCompetitionDescription;
        private TextInputEditText tietPopupCompetitionDescription;
        private Button btnPopupCompetitionSave;

        //Arguments
        private ManagementPopupAction managementPopupAction;
        private Competition competition;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Arguments != null)
            {
                int popupAction = Arguments.GetInt("popupAction", (int)ManagementPopupAction.Show);
                managementPopupAction = (ManagementPopupAction)popupAction;

                string data = Arguments.GetString("competition");
                if (!string.IsNullOrEmpty(data))
                    competition = JsonConvert.DeserializeObject<Competition>(data);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PopupCompetition, container, false);
            tilPopupCompetitionDescription = view.FindViewById<TextInputLayout>(Resource.Id.tilPopupCompetitionDescription);
            tietPopupCompetitionDescription = view.FindViewById<TextInputEditText>(Resource.Id.tietPopupCompetitionDescription);
            btnPopupCompetitionSave = view.FindViewById<Button>(Resource.Id.btnPopupCompetitionSave);

            btnPopupCompetitionSave.SetOnClickListener(this);

            MyLib.Instance.AddAfterTextChangeToTextInputLayout(tilPopupCompetitionDescription);

            switch (managementPopupAction)
            {
                case ManagementPopupAction.Show:
                    tietPopupCompetitionDescription.Enabled = tilPopupCompetitionDescription.Enabled = false;
                    btnPopupCompetitionSave.Visibility = ViewStates.Gone;
                    break;
                case ManagementPopupAction.Edit:
                    tietPopupCompetitionDescription.Text = competition?.Description;
                    break;
            }

            return view;
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnPopupCompetitionSave:
                    if (Validations())
                    {
                        switch (managementPopupAction)
                        {
                            case ManagementPopupAction.Edit:
                                if (competition != null)
                                {
                                    competition.Description = tietPopupCompetitionDescription.Text;
                                    bool isUpdated = await MyLib.Instance.UpdateObjectAsync(competition);
                                    if (isUpdated)
                                    {
                                        if (Activity != null)
                                            Toast.MakeText(Activity, Resource.String.competitionUpdated, ToastLength.Short).Show();
                                        Dismiss();
                                    }
                                    else if (Activity != null)
                                        Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                                }
                                break;
                            case ManagementPopupAction.Create:
                                Competition newCompetition = new Competition()
                                {
                                    Description = tietPopupCompetitionDescription.Text,
                                    State = true
                                };
                                bool isInserted = await MyLib.Instance.InsertObjectAsync(newCompetition);
                                if (isInserted)
                                {
                                    if (Activity != null)
                                        Toast.MakeText(Activity, Resource.String.competitionCreated, ToastLength.Short).Show();
                                    Dismiss();
                                }
                                else if (Activity != null)
                                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                                break;
                        }
                    }
                    break;
            }
        }

        private bool Validations()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(tietPopupCompetitionDescription.Text))
            {
                valid = false;
                tilPopupCompetitionDescription.Error = MyLib.Instance.GetString(Resource.String.emptyFieldError);
            }
            return valid;
        }
    }
}