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
using SistemaRH.Objects;
using System.Threading.Tasks;

namespace SistemaRH.Fragments
{
    public class WorkExperienceManagement : ManagementFragment, IManagementOperations
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            var workExperience = await MyLib.Instance.FindAllObjectsAsync<WorkExperience>();
            if (workExperience != null && workExperience.Count > 0)
            {
                foreach (var we in workExperience)
                {
                    items.Add(new ManagementItem()
                    {
                        Id = we.Id,
                        //Title = $"{we?.Name} ({we?.Username})",
                        Description = $"{MyLib.Instance.GetString(Resource.String.salary)}: {we?.Salary}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.enterprise)}: {we?.Enterprise}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.fromDate)}: {we?.FromDate.ToShortDateString()}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.toDate)}: {we?.ToDate.ToShortDateString()}"
                    });
                }        
            }
            return items;
        }

        public override void OnResume()
        {
            ManagementOperationsListener = this;
            base.OnResume();
        }

        public override void OnPause()
        {
            ManagementOperationsListener = null;
            base.OnPause();
        }

        public async Task RemoveObject(long objId)
        {
            await MyLib.Instance.DeleteObjectAsync<WorkExperience>(objId);
        }

        public Task AddObject(long objId)
        {
            throw new NotImplementedException();
        }
    }
}