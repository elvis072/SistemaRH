using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SistemaRH.Adapters;
using SistemaRH.Objects;
using SistemaRH.Utilities;

namespace SistemaRH.Activities
{
    [Activity(Label = "CandidateSkills", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CandidateSkills : AppCompatActivity, View.IOnClickListener
    {
        private RecyclerView rvCandidateSkillsCompetitions;
        private RecyclerView rvCandidateSkillsTrainings;
        private Button btnCandidateSkillsNext;
        private MultiCheckBoxAdapter multiCheckBoxAdapterCompetitions;
        private MultiCheckBoxAdapter multiCheckBoxAdapterTrainings;
        private List<MultiCheckBoxItem> multiCheckBoxItemsCompetitions;
        private List<MultiCheckBoxItem> multiCheckBoxItemsTrainings;
        private List<Competition> competetions;
        private List<Training> trainings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CandidateSkills);

            rvCandidateSkillsCompetitions = FindViewById<RecyclerView>(Resource.Id.rvCandidateSkillsCompetitions);
            rvCandidateSkillsTrainings = FindViewById<RecyclerView>(Resource.Id.rvCandidateSkillsTrainings);
            btnCandidateSkillsNext = FindViewById<Button>(Resource.Id.btnCandidateSkillsNext);

            btnCandidateSkillsNext.SetOnClickListener(this);

            rvCandidateSkillsCompetitions.SetItemViewCacheSize(40);
            rvCandidateSkillsTrainings.SetItemViewCacheSize(40);
            rvCandidateSkillsCompetitions.SetLayoutManager(new GridLayoutManager(this, 2));
            rvCandidateSkillsTrainings.SetLayoutManager(new GridLayoutManager(this, 2));

            multiCheckBoxItemsCompetitions = new List<MultiCheckBoxItem>();
            multiCheckBoxItemsTrainings = new List<MultiCheckBoxItem>();
            multiCheckBoxAdapterCompetitions = new MultiCheckBoxAdapter(multiCheckBoxItemsCompetitions);
            multiCheckBoxAdapterTrainings = new MultiCheckBoxAdapter(multiCheckBoxItemsTrainings);
            rvCandidateSkillsCompetitions.SetAdapter(multiCheckBoxAdapterCompetitions);
            rvCandidateSkillsTrainings.SetAdapter(multiCheckBoxAdapterTrainings);            

            GetData();
        }

        private async void GetData()
        {
            //Competitions
            competetions = await MyLib.Instance.FindAllObjectsAsync<Competition>();
            if (competetions != null && competetions.Count > 0)
            {
                foreach(var competetion in competetions)
                {
                    if (competetion?.State ?? false)                    
                        multiCheckBoxItemsCompetitions.Add(new MultiCheckBoxItem() { Description = competetion.Description });
                }
                multiCheckBoxAdapterCompetitions.NotifyItemRangeInserted(0, multiCheckBoxItemsCompetitions.Count);
                rvCandidateSkillsCompetitions.Animate().ScaleY(1.0f).SetDuration(100);
            }

            //Trainings
            trainings = await MyLib.Instance.FindAllObjectsAsync<Training>();
            if (trainings != null && trainings.Count > 0)
            {
                foreach (var training in trainings)
                {
                    multiCheckBoxItemsTrainings.Add(new MultiCheckBoxItem() {
                        Description = $"{MyLib.Instance.ConvertToDate(training.FromDate, training.ToDate)} " +
                        $"{training.Description} " +
                        $"{(string.IsNullOrEmpty(training.Institution) ? string.Empty : training.Institution)}" });
                }
                multiCheckBoxAdapterTrainings.NotifyItemRangeInserted(0, multiCheckBoxItemsTrainings.Count);
                rvCandidateSkillsTrainings.Animate().ScaleY(1.0f).SetDuration(100);
            }
        }

        public async void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnCandidateSkillsNext:
                    bool isCompetitionSelected = multiCheckBoxItemsCompetitions.Exists(x => x.IsChecked);
                    bool isTrainingSelected = multiCheckBoxItemsTrainings.Exists(x => x.IsChecked);
                    if (isCompetitionSelected && isTrainingSelected)
                    {
                        //Competitions
                        List<Competition> selectedCompetitions = new List<Competition>();              
                        foreach (var c in multiCheckBoxItemsCompetitions)
                        {
                            if (c.IsChecked)
                            {
                                int position = multiCheckBoxItemsCompetitions.IndexOf(c);
                                if (position >= 0 && position < multiCheckBoxItemsCompetitions.Count)
                                    selectedCompetitions.Add(competetions[position]);
                            }
                        }

                        //Trainigs
                        List<Training> selectedTrainings = new List<Training>();
                        foreach (var t in multiCheckBoxItemsTrainings)
                        {
                            if (t.IsChecked)
                            {
                                int position = multiCheckBoxItemsTrainings.IndexOf(t);
                                if (position >= 0 && position < multiCheckBoxItemsTrainings.Count)
                                    selectedTrainings.Add(trainings[position]);
                            }
                        }

                        var user = await MyLib.Instance.FindObjectAsync<User>(MyLib.Instance.GetUserId());
                        if (user != null)
                        {
                            var candidate = await MyLib.Instance.FindObjectAsync<Candidate>(user.CandidateId);
                            if (candidate != null)
                            {
                                candidate.Competitions = selectedCompetitions;
                                candidate.Trainings = selectedTrainings;
                                bool isUpdated = await MyLib.Instance.UpdateObjectAsync(candidate);                                
                                if (isUpdated)
                                {
                                    StartActivity(new Intent(this, typeof(CandidateExperience)));
                                    Finish();
                                    return;
                                }
                            }
                        }
                        Toast.MakeText(this, Resource.String.errorMessage, ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(this, Resource.String.minOneOptionMessageError, ToastLength.Short).Show();                                   
                    break;
            }            
        }      
    }
}