using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using SQLite;
using SistemaRH.Objects;
using SistemaRH.Activities;
using SQLiteNetExtensions.Extensions;

namespace SistemaRH.Utilities
{
    public class MyLib
    {
        private static MyLib myLib;

        public static MyLib Instance
        {
            get
            {
                if (myLib == null)
                    myLib = new MyLib();
                return myLib;
            }
        }

        public string GetString(int resId)
        {
            return Application.Context.GetString(resId);
        }

        public string EncryptText(string inputString)
        {
            byte[] data = Encoding.ASCII.GetBytes(inputString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);
            return hash;
        }

        public void AddAfterTextChangeToTextInputLayout(TextInputLayout textInputLayout)
        {
            textInputLayout.EditText.AfterTextChanged += (s, e) =>
            {
                if (textInputLayout.ErrorEnabled)
                    textInputLayout.ErrorEnabled = false;
            };
        }

        public string GetDBPath()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = path + "/" + Application.Context.PackageName + ".db3";
            return path;
        }

        public async Task<bool> TableExistAsync<T>() where T : new()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(GetDBPath());
                int count = await connection.Table<T>().CountAsync();
                connection.CloseAsync().GetAwaiter();
                return count != 0;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        public async Task<bool> CreateTables(Type[] types)
        {
            bool isSucessfull = false;
            try
            {
                await Task.Run(() =>
                {
                    var connection = new SQLiteConnection(GetDBPath());
                    connection.CreateTables(types: types);
                    connection.Close();
                    isSucessfull = true;
                });
            }
            catch (SQLiteException)
            {
            }
            return isSucessfull;
        }

        public async Task<bool> InsertObjectAsync<T>(T obj) where T : new()
        {
            bool isSucessfull = false;
            try
            {
                await Task.Run(async () =>
                {
                    var connection = new SQLiteConnection(GetDBPath());
                    if (!await TableExistAsync<T>())
                        connection.CreateTable<T>();
                    connection.InsertWithChildren(obj);
                    connection.Close();
                    isSucessfull = true;
                });
            }
            catch (SQLiteException e)
            {
            }
            return isSucessfull;
        }

        public async Task<bool> InsertObjectsAsync<T>(IEnumerable<T> objs) where T : new()
        {
            bool isSucessfull = false;
            try
            {
                await Task.Run(async () =>
                {
                    var connection = new SQLiteConnection(GetDBPath());
                    if (!await TableExistAsync<T>())
                        connection.CreateTable<T>();
                    connection.InsertAllWithChildren(objs);           
                    connection.Close();
                    isSucessfull = true;
                });
            }
            catch (SQLiteException)
            {
            }
            return isSucessfull;
        }

        public async Task<T> FindObjectAsync<T>(long obj_id) where T : new()
        {
            T result = default(T);
            try
            {
                await Task.Run(async () => 
                {
                    if (await TableExistAsync<T>())
                    {
                        var connection = new SQLiteConnection(GetDBPath());
                        result = connection.GetWithChildren<T>(obj_id);
                        connection.Close();
                    }
                });
            }
            catch (SQLiteException)
            {
            }
            return result;
        }

        public async Task<List<T>> FindAllObjectsAsync<T>() where T : new()
        {
            List<T> results = default(List<T>);
            try
            {
                await Task.Run(async () =>
                {
                    if (await TableExistAsync<T>())
                    {
                        var connection = new SQLiteConnection(GetDBPath());
                        results = connection.GetAllWithChildren<T>();
                        connection.Close();                        
                    }
                });

            }
            catch (SQLiteException)
            {
            }
            return results;
        }

        public async Task<List<T>> FindObjectsWithCustomQueryAsync<T>(List<string> paramsToReturn, List<KeyValuePair<string, string>> paramsToFind) where T : new()
        {
            try
            {
                if (await TableExistAsync<T>())
                {
                    var connection = new SQLiteAsyncConnection(GetDBPath());
                    string pReturn = string.Empty;
                    string pFind = string.Empty;

                    for (int i = 0; i < paramsToReturn.Count; i++)
                    {
                        if (i < paramsToReturn.Count - 1)
                            pReturn += paramsToReturn[i] + ", ";
                        else
                            pReturn += paramsToReturn[i];
                    }

                    for (int i = 0; i < paramsToFind.Count; i++)
                    {
                        if (i < paramsToFind.Count - 1)
                            pFind += paramsToFind[i].Key + "='" + paramsToFind[i].Value + "' AND ";
                        else
                            pFind += paramsToFind[i].Key + "='" + paramsToFind[i].Value + "'";
                    }
                    
                    var results = await connection.QueryAsync<T>("SELECT " + pReturn + " FROM " + typeof(T).Name + " WHERE " + pFind);
                    return results;
                }

            }
            catch (SQLiteException)
            {
            }
            return null;
        }

        public async Task<bool> UpdateObjectAsync<T>(T obj) where T : new()
        {
            bool isSucessfull = false;
            try
            {
                await Task.Run(async () =>
                {
                    if (await TableExistAsync<T>())
                    {
                        var connection = new SQLiteConnection(GetDBPath());
                        connection.UpdateWithChildren(obj);
                        connection.Close();
                        isSucessfull = true;
                    }
                });
            }
            catch (SQLiteException)
            {
            }
            return isSucessfull;
        }

        public async Task<bool> DeleteObjectAsync<T>(long obj_id) where T : new()
        {
            bool isSucessfull = false;
            try
            {
                if (await TableExistAsync<T>())
                {
                    var connection = new SQLiteAsyncConnection(GetDBPath());
                    int result = await connection.DeleteAsync<T>(obj_id);
                    isSucessfull = result > 0;
                    connection.CloseAsync().GetAwaiter();
                }
            }
            catch (SQLiteException)
            {
            }
            return isSucessfull;
        }

        public async Task<bool> DeleteAllObjectsAsync<T>() where T : new()
        {
            bool isSucessfull = false;
            try
            {
                if (await TableExistAsync<T>())
                {
                    var connection = new SQLiteAsyncConnection(GetDBPath());
                    int result = await connection.DeleteAllAsync<T>();
                    isSucessfull = result > 0;
                    connection.CloseAsync().GetAwaiter();
                }
            }
            catch (SQLiteException)
            {
            }
            return isSucessfull;
        }

        public void SaveUserId(long user_id)
        {
            Application.Context.GetSharedPreferences("user_data", FileCreationMode.Private).Edit().PutLong("user_id", user_id).Apply();
        }

        public long GetUserId()
        {
            long user_id = Application.Context.GetSharedPreferences("user_data", FileCreationMode.Private).GetLong("user_id", 0);
            return user_id;
        }

        public void OpenMainActivity(Activity activity)
        {
            activity?.StartActivity(new Intent(Application.Context, typeof(Main)));
            activity?.Finish();
        }

        public bool ValidateUsername(TextInputLayout textInputLayout)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(textInputLayout.EditText.Text))
            {
                valid = false;
                textInputLayout.Error = GetString(Resource.String.usernameCannotBeEmpty);
            }
            else
            {
                if (textInputLayout.EditText.Text.Length < 5)
                {
                    valid = false;
                    textInputLayout.Error = GetString(Resource.String.usernameMinLenght);
                }
            }
            return valid;
        }

        public bool ValidatePassword(TextInputLayout textInputLayout)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(textInputLayout.EditText.Text))
            {
                valid = false;
                textInputLayout.Error = GetString(Resource.String.passwordCannotBeEmpty);
            }
            else
            {
                if (textInputLayout.EditText.Text.Length < 8)
                {
                    valid = false;
                    textInputLayout.Error = GetString(Resource.String.passwordMinLenght);
                }
                else if (textInputLayout.EditText.Text.Distinct().Count() == 1)
                {
                    valid = false;
                    textInputLayout.Error = GetString(Resource.String.passwordDistintCharacters);
                }
            }
            return valid;
        }

        public bool ValidateConfirmPassword(TextInputLayout textInputLayout)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(textInputLayout.EditText.Text))
            {
                valid = false;
                textInputLayout.Error = GetString(Resource.String.passwordCannotBeEmpty);
            }
            else
            {
                if (!textInputLayout.EditText.Text.Equals(textInputLayout.EditText.Text))
                {
                    valid = false;
                    textInputLayout.Error = GetString(Resource.String.differentPasswordError);
                }
            }
            return valid;
        }

        public string ConvertToDate(DateTime from, DateTime to)
        {
            string result = string.Empty;

            if (from == null || to == null)
                return result;

            var diff = to - from;
            double totalDays = diff.TotalDays;
            if (totalDays > 30 && totalDays < (30 * 12))
                result = (int)(totalDays / 30) + " " + GetString(Resource.String.moths);
            else if (totalDays > (30 * 12))
                result = (int)(totalDays / 30 / 12) + " " + GetString(Resource.String.years);
            return result;
        }

        public void SignOut(Activity activity)
        {
            if (activity == null)
                return;

            SaveUserId(0);
            activity.StartActivity(new Intent(activity, typeof(Login)));
            activity.Finish();
        }
    }
}