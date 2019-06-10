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
    public class JobManagement : ManagementFragment, IManagementOperations
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
            return null;
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            jobs = await MyLib.Instance.FindAllObjectsAsync<Job>();
            if (jobs != null && jobs.Count > 0)
            {
                foreach (var j in jobs)
                {
                    if (j != null)
                        items.Add(new ManagementItem()
                        {
                            Id = j.Id,
                            Title = j.Name,
                            Description = $"{MyLib.Instance.GetString(Resource.String.riskLevel)}: {Enum.GetName(typeof(RiskLevel), j.RiskLevel)}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.minSalary)}: {j.MinSalary}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.maxSalary)}: {j.MaxSalary}",
                            State = j.State
                        });
                }
            }
            return items;
        }

        public async Task RemoveObject(long objId)
        {
            await MyLib.Instance.DeleteObjectAsync<Job>(objId);
        }

        public Task EditObject(long objId)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeObjectState(long objId)
        {
            var job = jobs.Where(x => x.Id == objId).FirstOrDefault();
            if (job != null)            
                await MyLib.Instance.UpdateObjectAsync(job);            
        }
    }
}