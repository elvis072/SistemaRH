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

namespace SistemaRH.Activities
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Main : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        private SupportToolbar toolbar;
        private DrawerLayout dlMain;
        private ActionBarDrawerToggle drawerToggle;
        private ListView lvMain;
        private ArrayAdapter<string> lvMainAdapter;
        private SupportFragment mCurrentFragment;
        private Stack<SupportFragment> mStackFragments;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            dlMain = FindViewById<DrawerLayout>(Resource.Id.dlMain);
            lvMain = FindViewById<ListView>(Resource.Id.lvMain);
            SetSupportActionBar(toolbar);

            mStackFragments = new Stack<SupportFragment>();
            var items = Application.Context.Resources.GetStringArray(Resource.Array.adminOptions);
            lvMainAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
            lvMain.Adapter = lvMainAdapter;
            lvMain.OnItemClickListener = this;
            lvMain.Tag = 0;

            drawerToggle = new MyActionBarDrawerToggle(this, dlMain, 0, 0) { DrawerIndicatorEnabled = true };
            dlMain.RemoveDrawerListener(drawerToggle);
            dlMain.AddDrawerListener(drawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            drawerToggle.SyncState();

            ShowFragment(new Home());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
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

        private void ShowFragment(SupportFragment fragment)
        {
            if (fragment == null || fragment.IsVisible)
                return;
                     
            var trans = SupportFragmentManager.BeginTransaction();
            if (mCurrentFragment != null)
            {
                trans.Hide(mCurrentFragment);          
                mStackFragments.Push(mCurrentFragment);
            }
            trans.Add(Resource.Id.flMainContainer, fragment, fragment.GetType().Name);       
            trans.Commit();        
            mCurrentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                mCurrentFragment = mStackFragments.Pop();
            }
            else
                base.OnBackPressed();
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            switch(position)
            {
                case (int)AdminOptions.CompetenctiesManagement:
                    CompetenctiesManagement competenctiesManagement = new CompetenctiesManagement();
                    competenctiesManagement.ManagementSwipeActions = ManagementSwipeActions.Delete;
                    ShowFragment(competenctiesManagement);
                    break;
                case (int)AdminOptions.LanguagesManagement:
                    break;
                case (int)AdminOptions.TrainingManagement:
                    TrainingManagement trainingManagement = new TrainingManagement();
                    trainingManagement.ManagementSwipeActions = ManagementSwipeActions.Delete;
                    ShowFragment(trainingManagement);
                    break;
                case (int)AdminOptions.JobsManagement:
                    JobsManagement jobsManagement = new JobsManagement();
                    jobsManagement.ManagementSwipeActions = ManagementSwipeActions.Delete;
                    ShowFragment(jobsManagement);
                    break;
            }
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