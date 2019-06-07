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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.Widget;
using Android.Content.PM;
using SistemaRH.Fragments;
using SistemaRH.Utilities;
using static SistemaRH.Enumerators.GlobalEnums;
using Android.Content.Res;
using SistemaRH.Controls;
using SistemaRH.Objects;

namespace SistemaRH.Activities
{
    [Activity(Label = "Main", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Main : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        private SupportToolbar toolbar;
        private DrawerLayout dlMain;
        private ActionBarDrawerToggle drawerToggle;
        private ListView lvMain;
        private ArrayAdapter<string> lvMainAdapter;
        private SupportFragment mCurrentFragment;
        private Stack<SupportFragment> mStackFragments;
        private Stack<string> mStackTitles;      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            dlMain = FindViewById<DrawerLayout>(Resource.Id.dlMain);
            lvMain = FindViewById<ListView>(Resource.Id.lvMain);
            SetSupportActionBar(toolbar);

            mStackFragments = new Stack<SupportFragment>();
            mStackTitles = new Stack<string>();
           
            ShowFragment(new Home(), nameof(Home));
            ShowOrHideAdminOptions();
        }

        private async void ShowOrHideAdminOptions()
        {
            var user = await MyLib.Instance.FindObjectAsync<User>(MyLib.Instance.GetUserId());
            if (user != null && user.Role == UsersRoles.Admin)
            {
                var items = Application.Context.Resources.GetStringArray(Resource.Array.adminOptions);
                lvMainAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
                lvMain.Adapter = lvMainAdapter;
                lvMain.OnItemClickListener = this;
                lvMain.BringToFront();
                lvMain.RequestLayout();

                drawerToggle = new ActionBarDrawerToggle(this, dlMain, 0, 0) { DrawerIndicatorEnabled = true };
                dlMain.RemoveDrawerListener(drawerToggle);
                dlMain.AddDrawerListener(drawerToggle);
                dlMain.FocusableInTouchMode = false;
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetDisplayShowTitleEnabled(true);
                drawerToggle.SyncState();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    dlMain.CloseDrawer(lvMain);
                    drawerToggle.OnOptionsItemSelected(item);
                    return true;
                case Resource.Id.action_signOut:
                    MyLib.Instance.SignOut(this);
                    return true; ;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        private void ShowFragment(SupportFragment fragment, string title)
        {
            if ((fragment?.IsVisible ?? true) || (mCurrentFragment?.GetType()?.Equals(fragment?.GetType()) ?? false))
                return;
                     
            var trans = SupportFragmentManager.BeginTransaction();
            if (mCurrentFragment != null)
            {
                trans.Hide(mCurrentFragment);          
                mStackFragments.Push(mCurrentFragment);
                mStackTitles.Push(SupportActionBar.Title);
            }
  
            trans.Add(Resource.Id.flMainContainer, fragment, fragment.GetType().Name);       
            trans.Commit();        
            mCurrentFragment = fragment;
            SupportActionBar.Title = title;
        }

        public override void OnBackPressed()
        {
            if (mStackFragments.Count > 0)
            {
                var trans = SupportFragmentManager.BeginTransaction();
                var fragment = mStackFragments.Pop();
                var title = mStackTitles.Pop();
                trans.Remove(mCurrentFragment);
                trans.Show(fragment);
                trans.Commit();
                mCurrentFragment = fragment;
                SupportActionBar.Title = title;
            }
            else
                base.OnBackPressed();
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            switch(position)
            {
                case (int)AdminOptions.CompetenctiesManagement:
                    CompetenceManagement competenctiesManagement = new CompetenceManagement { ManagementSwipeActions = ManagementSwipeActions.Delete };
                    ShowFragment(competenctiesManagement, MyLib.Instance.GetString(Resource.String.competitions));
                    break;
                case (int)AdminOptions.LanguagesManagement:
                    break;
                case (int)AdminOptions.TrainingManagement:
                    TrainingManagement trainingManagement = new TrainingManagement { ManagementSwipeActions = ManagementSwipeActions.Delete };
                    ShowFragment(trainingManagement, MyLib.Instance.GetString(Resource.String.trainings));
                    break;
                case (int)AdminOptions.JobsManagement:
                    JobManagement jobsManagement = new JobManagement { ManagementSwipeActions = ManagementSwipeActions.Delete };
                    ShowFragment(jobsManagement, MyLib.Instance.GetString(Resource.String.jobs));
                    break;
            }
            if (dlMain.IsDrawerOpen(lvMain))
                dlMain.CloseDrawer(lvMain);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            drawerToggle?.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            drawerToggle?.OnConfigurationChanged(newConfig);
        }
    }
}