using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimeraApp.Models;
using PrimeraApp.Pages;
using PrimeraApp.Services.Fire;
using System.Collections.ObjectModel;

namespace PrimeraApp.Viewmodels
{
    public partial class ListaCocheViewModel : BaseViewModel
    {
        private readonly string db_rama = "coches";
        private readonly string cancelButton = "Cancelar";
        private readonly string successTitle = "Éxito";
        private readonly string errorTitle = "Error";
        private readonly string okString = "Vale";


        [ObservableProperty]
        ObservableCollection<Coche> coches = new();
        [ObservableProperty]
        string matricula = string.Empty;
        [ObservableProperty]
        string marca = string.Empty;
        public ListaCocheViewModel()
        {
            Task.Run(GetCochesFromServer).GetAwaiter().GetResult();
            GetGamesRealTime();
        }
        public async Task GetCochesFromServer()
        {
            var res = (await FireDatabaseHelper.firebaseClient.Child(db_rama).OnceAsync<Coche>())
                .Select(d => new Coche()
                {
                    Matricula = d.Object.Matricula,
                    Marca = d.Object.Marca,
                    ID = d.Key
                }).ToList();
            coches.Clear();
            if (res != null)
            {
                foreach (Coche coche in res)
                {
                    coches.Add(coche);
                }
            }
        }

        [RelayCommand]
        async Task Add()
        {
            if (!string.IsNullOrEmpty(Marca) && !string.IsNullOrEmpty(Matricula))
            {
                Coche coche = new() { Matricula = Matricula, Marca = Marca };
                if (!coches.Contains(coche))
                {
                    coches.Add(coche);
                    await AddAsync(db_rama, coche);
                    await Application.Current.MainPage.DisplayAlert(successTitle, "Se han enviado los datos", okString);
                    Matricula = string.Empty;
                    Marca = string.Empty;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(errorTitle, "Ya existe en la base de datos", okString);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(errorTitle,"Rellena todos los campos",okString);
            }
        }

        [RelayCommand]
        async Task Tap(object item)
        {
            var coche = item as Coche;
            if (coche != null)
            {
                bool decision = await Application.Current.MainPage.DisplayAlert("Coche", "¿Qué quieres hacer con el coche " + coche.Matricula + "?", "Eliminar", "Modificar");
                if (decision)
                {
                    await DeleteAsync(db_rama, coche.ID);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Coche borrado", "OK");
                }
                else
                {
                    //Paso a la detailPage un ÚNICO parámetro
                    //await Shell.Current.GoToAsync($"{nameof(CocheDetail)}?name={coche.Matricula}");
                    //Paso a la detailPage VARIOS parámetros
                    var navigationParameter = new Dictionary<string, object>
                    {
                        { "Coche", coche },
                        { "NombreRama", db_rama }
                    };
                    await Shell.Current.GoToAsync($"{nameof(CocheDetail)}", navigationParameter);
                }
            }
        }
        private void GetGamesRealTime()
        {
            FireDatabaseHelper.firebaseClient
                .Child(db_rama)
                .AsObservable<Coche>()
                .Subscribe(d =>
                {
                    if (d.Object != null)
                    {
                        //Creación del objeto

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Coche coche = new ()
                            {
                                ID = d.Key,
                                Matricula = d.Object.Matricula,
                                Marca = d.Object.Marca
                            };
                            var existingGame = coches.FirstOrDefault(r => r.ID == d.Key);
                            if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                            {

                                //Insert
                                if (existingGame == null)
                                {
                                    coches.Add(coche);
                                }
                                //Update
                                else
                                {
                                    coches[coches.IndexOf(existingGame)] = coche;
                                }
                            }
                            //Delete
                            else if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
                            {
                                if (existingGame != null)
                                    coches.Remove(existingGame);
                            }
                        });
                    }
                });
        }
    }
}
