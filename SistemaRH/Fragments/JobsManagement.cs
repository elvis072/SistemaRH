using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SistemaRH.Adapters;
using SistemaRH.Objects;
using SistemaRH.Utilities;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Fragments
{
    public class JobsManagement : ManagementFragment, IManagementOperations
    {
        List<Job> jobs;

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

        public Task AddObject(long objId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            jobs = await MyLib.Instance.FindAllObjectsAsync<Job>();
            if (jobs != null && jobs.Count > 0)
            {
                foreach (var j in jobs)
                {
                    items.Add(new ManagementItem()
                    {
                        Id = j.Id,
                        Title = j?.Name,
                        Description = $"{MyLib.Instance.GetString(Resource.String.riskLevel)}: {Enum.GetName(typeof(RiskLevel), j?.RiskLevel)}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.minSalary)}: {j?.MinSalary}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.maxSalary)}: {j?.MaxSalary}"
                    });
                }
            }
            return items;
        }

        public async Task RemoveObject(long objId)
        {
            await MyLib.Instance.DeleteObjectAsync<Job>(objId);
        }
    }
}