using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using SistemaRH.Adapters;
using SistemaRH.Utilities;

namespace SistemaRH.Fragments
{
    public class Home : Fragment
    {
        private TabLayout tlHome;
        private ViewPager vpHome;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Home, container, false);
            tlHome = view.FindViewById<TabLayout>(Resource.Id.tlHome);
            vpHome = view.FindViewById<ViewPager>(Resource.Id.vpHome);

            Bundle candidateManagementArgs = new Bundle();
            candidateManagementArgs.PutString("fragment_title", MyLib.Instance.GetString(Resource.String.candidates));
            CandidateManagement candidateManagement = new CandidateManagement();
            candidateManagement.Arguments = candidateManagementArgs;

            Bundle workExperienceManagementArgs = new Bundle();
            workExperienceManagementArgs.PutString("fragment_title", MyLib.Instance.GetString(Resource.String.workExperience));
            WorkExperienceManagement workExperienceManagement = new WorkExperienceManagement();
            workExperienceManagement.Arguments = workExperienceManagementArgs;

            List<Fragment> fragments = new List<Fragment>()
            {
                candidateManagement,
                workExperienceManagement
            };           

            vpHome.Adapter = new MyFragmentPagerAdapter(ChildFragmentManager, fragments);
            tlHome.SetupWithViewPager(vpHome);       
            return view;
        }
    }
}