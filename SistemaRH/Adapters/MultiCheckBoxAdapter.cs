using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace SistemaRH.Adapters
{
    public class MultiCheckBoxAdapter : RecyclerView.Adapter
    {
        List<MultiCheckBoxItem> items;

        public MultiCheckBoxAdapter(List<MultiCheckBoxItem> items)
        {
            this.items = items;
        }

        public override int ItemCount => items.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            int id = Resource.Layout.MultiCheckBoxItem;
            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            return new MultiCheckBoxAdapterViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder Holder, int position)
        {
            var holder = Holder as MultiCheckBoxAdapterViewHolder;
            var item = items[position];

            if (holder != null && item != null)
            {
                holder.CbMultiCheckBoxItem.Text = item.Description;
            }
        }

        private class MultiCheckBoxAdapterViewHolder : RecyclerView.ViewHolder
        {
            public CheckBox CbMultiCheckBoxItem { get; set; }

            public MultiCheckBoxAdapterViewHolder(View itemView) : base(itemView)
            {
                CbMultiCheckBoxItem = itemView.FindViewById<CheckBox>(Resource.Id.cbMultiCheckBoxItem);
            }
        }
    }

    public class MultiCheckBoxItem
    {
        public string Description { get; set; }
    }
}