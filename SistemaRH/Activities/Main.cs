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

namespace SistemaRH.Activities
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Main : AppCompatActivity
    {
        private SupportToolbar toolbar;
        private DrawerLayout dlMain;
        private ActionBarDrawerToggle drawerToggle;
        private SupportFragment mCurrentFragment;
        private Stack<SupportFragment> mStackFragments;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            dlMain = FindViewById<DrawerLayout>(Resource.Id.dlMain);
            SetSupportActionBar(toolbar);

            mStackFragments = new Stack<SupportFragment>();      

            drawerToggle = new ActionBarDrawerToggle(this, dlMain, 0, 0) { DrawerIndicatorEnabled = true };
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
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
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
    }
}