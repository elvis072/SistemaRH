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
    public class CandidateSkills : AppCompatActivity
    {
        private RecyclerView rvCandidateSkillsCompetitions;
        private RecyclerView rvCandidateSkillsTrainings;
        private MultiCheckBoxAdapter multiCheckBoxAdapterCompetitions;
        private MultiCheckBoxAdapter multiCheckBoxAdapterTrainings;
        private List<MultiCheckBoxItem> multiCheckBoxItemsCompetitions;
        private List<MultiCheckBoxItem> multiCheckBoxItemsTrainings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CandidateSkills);

            rvCandidateSkillsCompetitions = FindViewById<RecyclerView>(Resource.Id.rvCandidateSkillsCompetitions);
            //rvCandidateSkillsTrainings = FindViewById<RecyclerView>(Resource.Id.rvCandidateSkillsTrainings);

            rvCandidateSkillsCompetitions.SetItemViewCacheSize(40);
            //rvCandidateSkillsTrainings.SetItemViewCacheSize(40);
            rvCandidateSkillsCompetitions.SetLayoutManager(new GridLayoutManager(this, 2));
            //rvCandidateSkillsTrainings.SetLayoutManager(new GridLayoutManager(this, 2, (int)GridOrientation.Vertical, false));

            multiCheckBoxItemsCompetitions = new List<MultiCheckBoxItem>();
            multiCheckBoxItemsTrainings = new List<MultiCheckBoxItem>();
            multiCheckBoxAdapterCompetitions = new MultiCheckBoxAdapter(multiCheckBoxItemsCompetitions);
            multiCheckBoxAdapterTrainings = new MultiCheckBoxAdapter(multiCheckBoxItemsTrainings);
            rvCandidateSkillsCompetitions.SetAdapter(multiCheckBoxAdapterCompetitions);
            //rvCandidateSkillsTrainings.SetAdapter(multiCheckBoxAdapterTrainings);

            GetData();
        }

        private async void GetData()
        {
            //Competitions
            var competetions = await MyLib.Instance.FindAllObjectsAsync<Competition>();
            if (competetions != null && competetions.Count > 0)
            {
                foreach(var competetion in competetions)
                {
                    multiCheckBoxItemsCompetitions.Add(new MultiCheckBoxItem() { Description = competetion.Description });
                }
                multiCheckBoxAdapterCompetitions.NotifyItemRangeInserted(0, multiCheckBoxItemsCompetitions.Count);
                competetions = null;
            }

            //Trainings
            var trainings = await MyLib.Instance.FindAllObjectsAsync<Training>();
            if (trainings != null && trainings.Count > 0)
            {
                foreach (var training in trainings)
                {
                    multiCheckBoxItemsTrainings.Add(new MultiCheckBoxItem() {
                        Description = $"{training.Description}\n " +
                        $"{MyLib.Instance.ConvertToDate(training.FromDate, training.ToDate)}" +
                        $"{(string.IsNullOrEmpty(training.Institution) ? string.Empty : "\n" + training.Institution)}" });
                }
                multiCheckBoxAdapterTrainings.NotifyItemRangeInserted(0, multiCheckBoxItemsTrainings.Count);
                trainings = null;
            }
        }
    }
}