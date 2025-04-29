using CommunityToolkit.Mvvm.ComponentModel;
using PrimeraApp.Services.Fire;
using Firebase.Database.Query;

namespace PrimeraApp.Viewmodels
{
    public class BaseViewModel : ObservableObject
    {
        public BaseViewModel() { }
        public async Task AddAsync<T>(string rama, T obj)
        {
            await FireDatabaseHelper.firebaseClient.Child(rama).PostAsync(obj);
        }

        public async Task DeleteAsync(string rama, string id)
        {
            await FireDatabaseHelper.firebaseClient.Child(rama).Child(id).DeleteAsync();
        }

        public async Task UpdateAsync<T>(string rama, string id, T obj)
        {
            await FireDatabaseHelper.firebaseClient.Child(rama).Child(id).PutAsync(obj);
        }
    }
}
