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

namespace SistemaRH.Fragments
{
    public class CompetenctiesManagement : ManagementFragment, IManagementOperations
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

        public Task AddObject(long objId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ManagementItem>> GetData()
        {
            List<ManagementItem> items = new List<ManagementItem>();
            competitions = await MyLib.Instance.FindAllObjectsAsync<Competition>();
            if (competitions != null && competitions.Count > 0)
            {
                foreach (var c in competitions)
                {
                    items.Add(new ManagementItem()
                    {
                        Id = c.Id,
                        Title = c?.Description
                    });
                }
            }
            return items;
        }

        public async Task RemoveObject(long objId)
        {
            await MyLib.Instance.DeleteObjectAsync<Competition>(objId);
        }
    }
}