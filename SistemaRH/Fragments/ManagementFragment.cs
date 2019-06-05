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

namespace SistemaRH.Fragments
{
    public interface IGetData
    {
        Task<List<ManagementItem>> GetData();        
    }

    public abstract class ManagementFragment : Fragment
    {
        private RecyclerView rvManagement;
        private ManagementAdapter rvManagementAdapter;
        private List<ManagementItem> rvManagementItems;
        private bool IsDataLoaded = false;

        public abstract IGetData GetDataListener { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Management, container, false);
            rvManagement = view.FindViewById<RecyclerView>(Resource.Id.rvManagement);
            rvManagement.SetItemViewCacheSize(40);
            rvManagement.SetLayoutManager(new LinearLayoutManager(container.Context, LinearLayoutManager.Vertical, false));
            rvManagementItems = new List<ManagementItem>();
            rvManagementAdapter = new ManagementAdapter(rvManagementItems);
            rvManagement.SetAdapter(rvManagementAdapter);
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
            if (GetDataListener == null)
                return;

            var data = await GetDataListener.GetData();
            if (data != null)
            {
                rvManagementItems.AddRange(data);
                rvManagementAdapter.NotifyItemRangeInserted(0, data.Count);
            }
        }
    }
}