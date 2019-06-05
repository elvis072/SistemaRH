using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace SistemaRH.Controls
{
    public class MyActionBarDrawerToggle : ActionBarDrawerToggle
    {
        private Activity activity;
        private int openResource;
        private int closeResource;

        public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
            this.activity = activity;
            openResource = openDrawerContentDescRes;
            closeResource = closeDrawerContentDescRes;
        }

        public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Android.Support.V7.Widget.Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
        }

        protected MyActionBarDrawerToggle(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnDrawerOpened(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerOpened(drawerView);
            }
        }

        public override void OnDrawerClosed(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerClosed(drawerView);
            }         
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            int drawerType = (int)drawerView.Tag;
            if (drawerType == 0)
            {
                base.OnDrawerSlide(drawerView, slideOffset);
            }  
        }
    }
}