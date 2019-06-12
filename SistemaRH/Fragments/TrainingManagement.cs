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
using Newtonsoft.Json;
using SistemaRH.Adapters;
using SistemaRH.Objects;
using SistemaRH.Popups;
using SistemaRH.Utilities;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Fragments
{
    public class TrainingManagement : ManagementFragment, IManagementOperations
    {
        List<Training> trainings;

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
            return Task.Run(() => { ShowPopupTraining(item, ManagementPopupAction.Create); });
        }

        public async Task<List<ManagementItem>> GetData()
        {
            var trainingLevels = Application.Context.Resources.GetStringArray(Resource.Array.trainingLevels);
            List<ManagementItem> items = new List<ManagementItem>();
            trainings = await MyLib.Instance.FindAllObjectsAsync<Training>();
            if (trainings != null && trainings.Count > 0)
            {
                foreach (var t in trainings)
                {
                    if (t != null)
                        items.Add(new ManagementItem()
                        {
                            Id = t.Id,
                            Title = t.Description,
                            Description = $"{MyLib.Instance.GetString(Resource.String.trainingLevel)}: {trainingLevels[(int)t.TrainingLevel]}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.institution)}: {t.Institution}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.fromDate)}: {t.FromDate.ToShortDateString()}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.toDate)}: {t.ToDate.ToShortDateString()}",
                            State = t.State
                        });
                }
            }
            return items;
        }

        public async Task RemoveItem(ManagementItem item)
        {
            if (item == null)
                return;

            await MyLib.Instance.DeleteObjectAsync<Training>(item.Id);
        }

        public Task EditItem(ManagementItem item)
        {
            if (item == null)
                return null;

            return Task.Run(() => { ShowPopupTraining(item, ManagementPopupAction.Edit); });
        }

        public async Task ChangeItemState(ManagementItem item)
        {
            if (item == null)
                return;

            var training = trainings.Where(x => x.Id == item.Id).FirstOrDefault();
            if (training != null)
            {
                training.State = item.State;
                await MyLib.Instance.UpdateObjectAsync(training);
            }
        }

        private void ShowPopupTraining(ManagementItem item, ManagementPopupAction popupAction)
        {
            var training = item != null ? trainings.Where(x => x.Id == item.Id).FirstOrDefault() : null;
            if (training != null || (training == null && popupAction == ManagementPopupAction.Create))
            {
                var ft = ChildFragmentManager.BeginTransaction();
                PopupTraining popupJob = new PopupTraining();
                Bundle args = new Bundle();
                args.PutString("training", training != null ? JsonConvert.SerializeObject(training) : null);
                args.PutInt("popupAction", (int)popupAction);
                popupJob.Arguments = args;

                ft.Add(popupJob, nameof(PopupTraining));
                ft.Commit();
            }
        }
    }
}