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
    public class CandidateManagement : ManagementFragment, IManagementOperations
    {
        List<Candidate> candidates;

        public async Task<List<ManagementItem>> GetData()
        {           
            List<ManagementItem> items = new List<ManagementItem>();
            var emple = await MyLib.Instance.FindAllObjectsAsync<Employee>();
            candidates = await MyLib.Instance.FindAllObjectsAsync<Candidate>();
            if (candidates != null && candidates.Count > 0)
            {
                foreach (var c in candidates)
                {
                    if (c != null)
                    {
                        string competitions = string.Empty;
                        for (int i = 0; i < c.Competitions.Count; i++)
                        {
                            var comp = c.Competitions[i];
                            if (comp == null)
                                continue;

                            if (i < c.Competitions.Count - 1)
                                competitions += (comp.Description + ", ");
                            else
                                competitions += comp.Description;
                        }

                        string trainings = string.Empty;
                        for (int i = 0; i < c.Trainings.Count; i++)
                        {
                            var training = c.Trainings[i];
                            if (training == null)
                                continue;

                            string tempTrain = $"{MyLib.Instance.ConvertToDate(training.FromDate, training.ToDate)} {training.Description}";
                            if (i < c.Trainings.Count - 1)
                                trainings += (tempTrain + ", ");
                            else
                                trainings += tempTrain;
                        }                        

                        items.Add(new ManagementItem()
                        {
                            Id = c.Id,
                            Title = $"{c.Name}",
                            Description = $"{MyLib.Instance.GetString(Resource.String.job)}: {c.ExpectedJob?.Name}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.expectedSalary)}: {c.ExpectedSalary}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.department)}: {c.Department?.Description}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.competitions)}: {competitions}\n" +
                                          $"{MyLib.Instance.GetString(Resource.String.trainings)}: {trainings}\n" +
                                          (!string.IsNullOrEmpty(c.RecommendatedBy) ? $"{MyLib.Instance.GetString(Resource.String.recommendatedBy)}: {c.RecommendatedBy}\n" : string.Empty)
                        });
                    }
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

        public async Task RemoveItem(ManagementItem item)
        {
            if (item == null)
                return;

            var candidate = candidates.Where(x => x.Id == item.Id).FirstOrDefault();
            if (candidate != null)
            {
                await MyLib.Instance.DeleteObjectAsync<Candidate>(item.Id);
                await MyLib.Instance.DeleteObjectAsync<User>(candidate.User.Id);               
            }
            else
                Activity?.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                });
        }

        public async Task AddItem(ManagementItem item)
        {
            if (item == null)
                return;

            var candidate = candidates.Where(x => x.Id == item.Id).FirstOrDefault();
            if (candidate != null)
            {
                candidate.User.Role = Enumerators.GlobalEnums.UsersRoles.Employee;
                await MyLib.Instance.UpdateObjectAsync(candidate.User);
                Employee newEmployee = new Employee()
                {
                    User = candidate.User,
                    IdentificationCard = candidate.IdentificationCard,
                    Name = candidate.Name,
                    EntryDate = DateTime.Now,
                    Department = candidate.Department,
                    Job = candidate.ExpectedJob,
                    MensualSalary = candidate.ExpectedSalary,
                    State = true
                };
                await MyLib.Instance.InsertObjectAsync(newEmployee);
                await MyLib.Instance.DeleteObjectAsync<Candidate>(item.Id);

                Activity?.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, Resource.String.candidateAccepted, ToastLength.Short).Show();
                });
            }
            else
                Activity?.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, Resource.String.errorMessage, ToastLength.Short).Show();
                });            
        }

        public Task EditItem(ManagementItem item)
        {
            return null;
        }

        public Task ChangeItemState(ManagementItem item)
        {
            return null;
        }
    }
}