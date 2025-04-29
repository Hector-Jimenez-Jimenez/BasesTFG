using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using PrimeraApp.Models;
using PrimeraApp.Pages.Popups;
using PrimeraApp.Services.Fire;
using System.Collections.ObjectModel;
using PrimeraApp.Viewmodels.Popups;

namespace PrimeraApp.Viewmodels
{
    public partial class ListaViewModel : BaseViewModel    {
        //private readonly FireDatabaseHelper _database = new();
        public static System.IO.Stream? ImagenProfile { get; set; } = null;
        [ObservableProperty]
        ObservableCollection<Persona> personas = new();
        [ObservableProperty]
        ImageSource? imageSourceStream;
        private readonly string db_rama = "personas";

        public ListaViewModel()
        {
            ImageSourceStream = ImageSource.FromFile("dotnet_bot.png");
            //AddPeople();
            //Task.Run(AddAsync).GetAwaiter().GetResult();
            //Task.Run(GetDataAsync).GetAwaiter().GetResult();
        }
        private void AddPeople()
        {
            personas.Add(new() { Nombre = "Luis", Apellido = "Martinez", DNI = "123456789A"});
            personas.Add(new() { Nombre = "Andrés", Apellido = "Garmendia", DNI = "103456789A" });
            personas.Add(new() { Nombre = "Alejandro", Apellido = "Bueno", DNI = "120456789A" });
            personas.Add(new() { Nombre = "Diego", Apellido = "Asensio", DNI = "123056789A" });
            personas.Add(new() { Nombre = "Cristina", Apellido = "Celimendiz", DNI = "123406789A" });
            personas.Add(new() { Nombre = "Pepito", Apellido = "Grillo", DNI = "123450789A" });
        }

        private async Task AddAsync()
        {
            foreach (Persona persona in personas)
            {
                await AddAsync(db_rama,persona);
            }
        }

        private async Task GetDataAsync()
        {
            var res = (await FireDatabaseHelper.firebaseClient.Child(db_rama).OnceAsync<Persona>())
                .Select(d => new Persona()
            {
                Nombre = d.Object.Nombre,
                Apellido = d.Object.Apellido,
                DNI = d.Object.DNI,
                ID = d.Key
            }).ToList();
            personas.Clear();
            if (res != null)
            {
                foreach (Persona persona in res)
                {
                    personas.Add(persona);
                }
            }
        }
        [RelayCommand]
        async Task ChangePhoto()
        {
            PhotoViewModel viewModel = new();
            var popup = new PhotoPopUp(viewModel);
            try
            {
                await Shell.Current.ShowPopupAsync(popup);
                ImageSourceStream = viewModel.ImageSource;
                popup.Close();
            }
            catch (ObjectDisposedException)
            {
                ImageSourceStream = viewModel.ImageSource;
                popup.Close();
                
            }
            
            /*if (ImagenProfile != null)
            {
                ImageSourceStream = ImageSource.FromStream(() => ImagenProfile);
            }*/
        }
    }
}
