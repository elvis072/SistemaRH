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

        public Task AddObject(long objId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            trainings = await MyLib.Instance.FindAllObjectsAsync<Training>();
            if (trainings != null && trainings.Count > 0)
            {
                foreach (var t in trainings)
                {
                    items.Add(new ManagementItem()
                    {
                        Id = t.Id,
                        Title = t?.Description,
                        Description = $"{MyLib.Instance.GetString(Resource.String.trainingLevel)}: {Enum.GetName(typeof(TrainingLevel), t?.TrainingLevel)}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.institution)}: {t?.Institution}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.fromDate)}: {t?.FromDate.ToShortDateString()}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.toDate)}: {t?.ToDate.ToShortDateString()}"
                    });
                }
            }
            return items;
        }

        public async Task RemoveObject(long objId)
        {
            await MyLib.Instance.DeleteObjectAsync<Training>(objId);
        }
    }
}