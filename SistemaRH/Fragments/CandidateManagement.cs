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
    public class CandidateManagement : ManagementFragment, IGetData
    {
        public override IGetData GetDataListener => this;

        public async Task<List<ManagementItem>> GetData()
        {           
            List<ManagementItem> items = new List<ManagementItem>();
            var candidates = await MyLib.Instance.FindAllObjectsAsync<Candidate>();
            if (candidates != null && candidates.Count > 0)
            {
                foreach (var c in candidates)
                {
                    items.Add(new ManagementItem()
                    {
                        Title = $"{c?.Name} ({c?.Username})",
                        Description = $"{MyLib.Instance.GetString(Resource.String.job)}: {c?.ExpectedJob?.Name}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.expectedSalary)}: {c?.ExpectedSalary}\n" +
                                      $"{MyLib.Instance.GetString(Resource.String.department)}: {c?.Department?.Description}"
                    });
                }
            }
            else
                Activity?.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                });
            return items;
        }
    }
}