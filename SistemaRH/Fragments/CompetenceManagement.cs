using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Newtonsoft.Json;
using SistemaRH.Adapters;
using SistemaRH.Objects;
using SistemaRH.Popups;
using SistemaRH.Utilities;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Fragments
{
    public class CompetenceManagement : ManagementFragment, IManagementOperations
    {
        List<Competition> competitions;

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
            return Task.Run(() => { ShowPopupCompetition(item, ManagementPopupAction.Create); });
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            competitions = await MyLib.Instance.FindAllObjectsAsync<Competition>();
            if (competitions != null && competitions.Count > 0)
            {
                foreach (var c in competitions)
                {
                    if (c != null)
                        items.Add(new ManagementItem()
                        {
                            Id = c.Id,
                            Title = c.Description,
                            State = c.State
                        });
                }
            }
            return items;
        }

        public async Task RemoveItem(ManagementItem item)
        {
            if (item == null)
                return;

            await MyLib.Instance.DeleteObjectAsync<Competition>(item.Id);
        }

        public Task EditItem(ManagementItem item)
        {
            if (item == null)
                return null;

            return Task.Run(() => { ShowPopupCompetition(item, ManagementPopupAction.Edit); });
        }

        public async Task ChangeItemState(ManagementItem item)
        {
            if (item == null)
                return;

            var competition = competitions.Where(x => x.Id == item.Id).FirstOrDefault();
            if (competition != null)
            {
                competition.State = item.State;
                await MyLib.Instance.UpdateObjectAsync(competition);
            }
        }

        private void ShowPopupCompetition(ManagementItem item, ManagementPopupAction popupAction)
        {
            var competition = item != null ? competitions.Where(x => x.Id == item.Id).FirstOrDefault() : null;
            if (competition != null || (competition == null && popupAction == ManagementPopupAction.Create))
            {
                var ft = ChildFragmentManager.BeginTransaction();
                PopupCompetition popupJob = new PopupCompetition();
                Bundle args = new Bundle();
                args.PutString("competition", competition != null ? JsonConvert.SerializeObject(competition) : null);
                args.PutInt("popupAction", (int)popupAction);
                popupJob.Arguments = args;

                ft.Add(popupJob, nameof(PopupCompetition));
                ft.Commit();
            }
        }
    }
}