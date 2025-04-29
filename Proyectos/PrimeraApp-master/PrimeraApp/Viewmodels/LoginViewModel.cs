using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimeraApp.Pages.Popups;
using PrimeraApp.Services.Fire;

namespace PrimeraApp.Viewmodels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        string username = string.Empty;
        [ObservableProperty]
        string password = string.Empty;
        public LoginViewModel()
        {
            Task.Run(AutomaticLogin).GetAwaiter().GetResult();
        }

        async Task RetrieveLogin()
        {
            var name = await SecureStorage.GetAsync("Username");
            var pass = await SecureStorage.GetAsync("Password");
            if (name != null && pass != null)
            {
                Username = name;
                Password = pass;
            }
        }

        async Task AutomaticLogin()
        {
            var name = await SecureStorage.GetAsync("Username");
            var pass = await SecureStorage.GetAsync("Password");
            if (name != null && pass != null)
            {
                Username = name;
                Password = pass;
                try
                {
                    await LoginAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new AppShell()));
                    //Application.Current.MainPage = new AppShell();
                }
                catch (Exception)
                {
                    SecureStorage.RemoveAll();
                    await Application.Current.MainPage.DisplayAlert("Error", "Revisa las credecenciales", "OK");
                }
            }
        }
        [RelayCommand]
        async Task ClickedButton()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Revisa los campos", "OK");
            }
            else
            {
                try
                {
                    await ShowPopup();
                    await LoginAsync();
                    await SecureStorage.SetAsync("Username", Username);
                    await SecureStorage.SetAsync("Password", Password);
                    Application.Current.MainPage = new AppShell();
                }
                catch (Exception)
                {
                    SecureStorage.RemoveAll();
                    Username = string.Empty;
                    Password = string.Empty;
                    await Application.Current.MainPage.DisplayAlert("Error", "Revisa las credecenciales", "OK");
                }
            }
        }
        async Task LoginAsync()
        {
            await FireAuthHelper.client.SignInWithEmailAndPasswordAsync(Username, Password);
        }
        async Task ShowPopup()
        {
            var popup = new SpinnerPopup();
            Application.Current.MainPage.ShowPopup(popup);
            await Task.Delay(5000);
            popup.Close();
        }
    }
}
