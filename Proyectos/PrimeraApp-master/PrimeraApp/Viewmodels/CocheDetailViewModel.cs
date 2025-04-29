using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimeraApp.Models;

namespace PrimeraApp.Viewmodels
{
    [QueryProperty(nameof(Db_rama), "NombreRama")]
    [QueryProperty(nameof(Coche), "Coche")]
    
    public partial class CocheDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        Coche coche = new();
        [ObservableProperty]
        string db_rama = "";

        public CocheDetailViewModel() { }

        [RelayCommand]
        async Task Press()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Modificar", "¿Quiere mandar los cambios?", "Enviar", "Cancelar");
            if (result)
            {
                //Mandamos los cambios actualizados a la base de datos
                await UpdateAsync(Db_rama, coche.ID, coche);
                //Volvemos hacia atrás
                await Shell.Current.GoToAsync("..");
            }
            
        }
    }
}
