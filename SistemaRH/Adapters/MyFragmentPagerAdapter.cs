using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;

namespace SistemaRH.Adapters
{
    public class MyFragmentPagerAdapter : FragmentPagerAdapter
    {
        List<Fragment> fragments;

        public MyFragmentPagerAdapter(FragmentManager fm, List<Fragment> fragments) : base(fm)
        {
            this.fragments = fragments;
        }

        public override int Count
        {
            get
            {
                return fragments.Count;
            }
        }
       

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new StringBuilder(fragments[position].Arguments.GetString("fragment_title"));
        }
    }
}