using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Content;
using SistemaRH.Utilities;
using SistemaRH.Fragments;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Adapters
{
    public class ManagementAdapter : RecyclerView.Adapter
    {
        public List<ManagementItem> Items;
        public ManagementFragment Fragment;

        public ManagementAdapter(List<ManagementItem> items, ManagementFragment fragment)
        { 
            Items = items;
            Fragment = fragment;
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
            return new ManagementAdapterViewHolder(itemView, this);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as ManagementAdapterViewHolder;
            var item = Items[position];
            if (holder != null && item != null)
            {
                holder.SetItem(item);
                holder.TvManagementItemTitle.Text = item.Title;
                holder.TvManagementItemDescription.Text = item.Description;
                holder.SwManagementItem.Checked = item.State;

                if (!item.State)
                    holder.ItemView.Alpha = 0.5f;
                else
                    holder.ItemView.Alpha = 1.0f;

                if (Fragment.ManagementItemOptions == ManagementItemOptions.All)
                    holder.SwManagementItem.Visibility = ViewStates.Visible;
                else
                    holder.SwManagementItem.Visibility = ViewStates.Gone;
            }
        }

        private class ManagementAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public TextView TvManagementItemTitle { get; set; }
            public TextView TvManagementItemDescription { get; set; }
            public Switch SwManagementItem { get; set; }

            private ManagementAdapter adapter;
            private ManagementItem item;

            public ManagementAdapterViewHolder(View itemView, ManagementAdapter adapter) : base(itemView)
            {
                this.adapter = adapter;
                TvManagementItemTitle = itemView.FindViewById<TextView>(Resource.Id.tvManagementItemTitle);
                TvManagementItemDescription = itemView.FindViewById<TextView>(Resource.Id.tvManagementItemDescription);
                SwManagementItem = itemView.FindViewById<Switch>(Resource.Id.swManagementItem);
                ItemView.SetOnClickListener(this);
                SwManagementItem.SetOnClickListener(this);
            }

            public void SetItem(ManagementItem item)
            {
                this.item = item;
            }

            public void OnClick(View v)
            {
                if (v == ItemView)
                    adapter.Fragment.ManagementOperationsListener?.EditItem(item);

                switch(v.Id)
                {
                    case Resource.Id.swManagementItem:
                        item.State = SwManagementItem.Checked;
                        adapter.NotifyItemChanged(AdapterPosition);
                        adapter.Fragment.ManagementOperationsListener?.ChangeItemState(item)?.GetAwaiter();
                        break;           
                }
            }
        }
    }

    public class ManagementItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool State { get; set; } = true;
    }
}