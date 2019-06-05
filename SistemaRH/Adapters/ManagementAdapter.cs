using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Content;
using SistemaRH.Utilities;
using SistemaRH.Fragments;

namespace SistemaRH.Adapters
{
    public class ManagementAdapter : RecyclerView.Adapter
    {
        public List<ManagementItem> Items;
        public ManagementFragment Parent;

        public ManagementAdapter(List<ManagementItem> items, ManagementFragment parent)
        { 
            Items = items;
            Parent = parent;
        }

        public override int ItemCount
        {
            get
            {
                return Items.Count;
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
            var item = Items[position];
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
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}