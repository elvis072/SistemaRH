using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace SistemaRH.Adapters
{
    public class ManagementAdapter : RecyclerView.Adapter
    {
        List<ManagementItem> items;

        public ManagementAdapter(List<ManagementItem> items)
        {
            this.items = items;
        }

        public override int ItemCount
        {
            get
            {
                return items.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            int id = Resource.Layout.ManagementItem;
            var itemView = LayoutInflater.FromContext(parent.Context).Inflate(id, parent, false);
            return new ManagementAdapterViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as ManagementAdapterViewHolder;
            var item = items[position];
            if (holder != null && item != null)
            {
                holder.TvManagementItemTitle.Text = item.Title;
                holder.TvManagementItemDescription.Text = item.Description;
            }
        }

        private class ManagementAdapterViewHolder : RecyclerView.ViewHolder
        {
            public TextView TvManagementItemTitle { get; set; }
            public TextView TvManagementItemDescription { get; set; }

            public ManagementAdapterViewHolder(View itemView) : base(itemView)
            {
                TvManagementItemTitle = itemView.FindViewById<TextView>(Resource.Id.tvManagementItemTitle);
                TvManagementItemDescription = itemView.FindViewById<TextView>(Resource.Id.tvManagementItemDescription);
            }
        }
    }

    public class ManagementItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}