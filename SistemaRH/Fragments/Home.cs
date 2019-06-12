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
using static SistemaRH.Enumerators.GlobalEnums;
using SistemaRH.Objects;

namespace SistemaRH.Fragments
{
    public class Home : Fragment
    {
        private TabLayout tlHome;
        private ViewPager vpHome;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Home, container, false);
            tlHome = view.FindViewById<TabLayout>(Resource.Id.tlHome);
            vpHome = view.FindViewById<ViewPager>(Resource.Id.vpHome);  
            SetData();
            return view;
        }

        private async void SetData()
        {
            User user = await MyLib.Instance.FindObjectAsync<User>(MyLib.Instance.GetUserId());
            bool isEmployee = user.Role == UsersRoles.Employee;

            Bundle candidateManagementArgs = new Bundle();
            candidateManagementArgs.PutString("fragment_title", MyLib.Instance.GetString(Resource.String.candidates));
            CandidateManagement candidateManagement = new CandidateManagement
            {
                ManagementSwipeActions = isEmployee ? ManagementSwipeActions.DeleteAndAdd : ManagementSwipeActions.None,
                ManagementItemOptions = ManagementItemOptions.OnlyCandidateOptions,
                Arguments = candidateManagementArgs
            };

            Bundle workExperienceManagementArgs = new Bundle();
            workExperienceManagementArgs.PutString("fragment_title", MyLib.Instance.GetString(Resource.String.workExperience));
            WorkExperienceManagement workExperienceManagement = new WorkExperienceManagement
            {
                ManagementSwipeActions = isEmployee ? ManagementSwipeActions.Delete : ManagementSwipeActions.None,
                ManagementItemOptions = ManagementItemOptions.OnlyCandidateOptions,
                Arguments = workExperienceManagementArgs
            };

            var fragments = new List<Fragment>()
            {
                candidateManagement,
                workExperienceManagement
            };

            vpHome.Adapter = new MyFragmentPagerAdapter(ChildFragmentManager, fragments);
            tlHome.SetupWithViewPager(vpHome);
        }
    }
}