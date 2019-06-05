using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using SistemaRH.Adapters;
using SistemaRH.Utilities;
using System.Threading.Tasks;
using Android.Support.V7.Widget.Helper;
using SistemaRH.Controls;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Fragments
{
    public interface IManagementOperations
    {
        Task<List<ManagementItem>> GetData();
        Task RemoveObject(long objId);
        Task AddObject(long objId);
    }

    public class ManagementFragment : Fragment
    {
        private TextView tvManagementNotContent;
        private RecyclerView rvManagement;
        private ManagementAdapter rvManagementAdapter;
        private List<ManagementItem> rvManagementItems;
        private bool IsDataLoaded = false;

        public IManagementOperations ManagementOperationsListener { set; get; }

        public ManagementSwipeActions ManagementSwipeActions { get; set; } = ManagementSwipeActions.None;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Management, container, false);
            tvManagementNotContent = view.FindViewById<TextView>(Resource.Id.tvManagementNotContent);
            rvManagement = view.FindViewById<RecyclerView>(Resource.Id.rvManagement);
            rvManagement.SetItemViewCacheSize(40);
            rvManagement.SetLayoutManager(new LinearLayoutManager(container.Context, LinearLayoutManager.Vertical, false));
            rvManagementItems = new List<ManagementItem>();
            rvManagementAdapter = new ManagementAdapter(rvManagementItems, this);
            rvManagement.SetAdapter(rvManagementAdapter);

            if (ManagementSwipeActions != ManagementSwipeActions.None)
            {
                ItemTouchHelper itemTouchHelper = new ItemTouchHelper(new ManagementSwipeToDeleteCallback(rvManagementAdapter, ManagementSwipeActions));
                itemTouchHelper.AttachToRecyclerView(rvManagement);
            }
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (!IsDataLoaded)
                SetData();
        }

        private async void SetData()
        {
            if (ManagementOperationsListener == null)
                return;

            var data = await ManagementOperationsListener.GetData();
            if (data != null && data.Count > 0)
            {
                SetNotContentVisibility(false);
                rvManagementItems.AddRange(data);
                rvManagementAdapter.NotifyItemRangeInserted(0, data.Count);
            }
            else
                SetNotContentVisibility(true);
        }

        public void SetNotContentVisibility(bool visible)
        {
            if (visible)
            {
                rvManagement.Visibility = ViewStates.Gone;
                tvManagementNotContent.Visibility = ViewStates.Visible;
            }
            else
            {
                tvManagementNotContent.Visibility = ViewStates.Gone;
                rvManagement.Visibility = ViewStates.Visible;
            }
        }
    }
}