using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
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
            return Task.Run(() => { ShowPopupJob(item, ManagementPopupAction.Create); });
        }

        public async Task<List<ManagementItem>> GetData()
        {
            var riskLevels = Application.Context.Resources.GetStringArray(Resource.Array.riskLevels);
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
                            Description = $"{MyLib.Instance.GetString(Resource.String.riskLevel)}: {riskLevels[(int)j.RiskLevel]}\n" +
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

            return Task.Run(() => { ShowPopupJob(item, ManagementPopupAction.Edit); });
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

        private void ShowPopupJob(ManagementItem item, ManagementPopupAction popupAction)
        {
            var job = item != null ? jobs.Where(x => x.Id == item.Id).FirstOrDefault() : null;
            if (job != null || (job == null && popupAction == ManagementPopupAction.Create))
            {
                var ft = ChildFragmentManager.BeginTransaction();
                PopupJob popupJob = new PopupJob();
                Bundle args = new Bundle();
                args.PutString("job", job != null ? JsonConvert.SerializeObject(job) : null);
                args.PutInt("popupAction", (int)popupAction);
                popupJob.Arguments = args;

                ft.Add(popupJob, nameof(PopupJob));
                ft.Commit();
            }
        }
    }
}