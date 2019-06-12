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
using SistemaRH.Popups;
using Newtonsoft.Json;

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

        public Task AddItem(ManagementItem item)
        {
            return null;
        }

        public async Task<List<ManagementItem>> GetData()
        {
            var riskLevel = Application.Context.Resources.GetStringArray(Resource.Array.riskLevels);
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
                            Description = $"{MyLib.Instance.GetString(Resource.String.riskLevel)}: {riskLevel[(int)j.RiskLevel]}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.minSalary)}: {j.MinSalary}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.maxSalary)}: {j.MaxSalary}",
                            State = j.State
                        });
                }
            }
            return items;
        }

        public async Task RemoveItem(ManagementItem item)
        {
            if (item == null)
                return;

            await MyLib.Instance.DeleteObjectAsync<Job>(item.Id);
        }

        public Task EditItem(ManagementItem item)
        {
            if (item == null)
                return null;

            Job job = jobs.Where(x => x.Id == item.Id).FirstOrDefault();
            ShowPopupJob(job);
            return null;    
        }

        public async Task ChangeItemState(ManagementItem item)
        {
            if (item == null)
                return;

            var job = jobs.Where(x => x.Id == item.Id).FirstOrDefault();
            if (job != null)
            {
                job.State = item.State;
                await MyLib.Instance.UpdateObjectAsync(job);
            }
        }

        private void ShowPopupJob(Job job)
        {
            if (job == null)
                return;

            var ft = ChildFragmentManager.BeginTransaction();
            PopupJob popupJob = new PopupJob();
            Bundle args = new Bundle();
            args.PutString("job", JsonConvert.SerializeObject(job));
            args.PutInt("popupAction", (int)ManagementPopupAction.Edit);
            popupJob.Arguments = args;                       

            ft.Add(popupJob, nameof(PopupJob));
            ft.Commit();     
        }
    }
}