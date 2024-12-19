using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;



namespace ToDo_CostaRica.ViewModels.Login
{
    public class ForgotViewModel : ViewModelBase
    {
        string email;
        public AsyncCommand LoginCommand { get; }
        public AsyncCommand CerrarCommand { get; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public ForgotViewModel()
        {
            LoginCommand = new AsyncCommand(OnLoginClicked, allowsMultipleExecutions: false);
            CerrarCommand = new AsyncCommand(CerrarClicked, allowsMultipleExecutions: false);
           // CrossSimpleAudioPlayer.Current.Load("error.wav");
        }
        
        async Task CerrarClicked()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async Task OnLoginClicked()
        {
            if (!email.EsEmail())
            {
                var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));

                audioPlayer.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Necesitas un email válido!");
                return;
            }

            try
            {
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Chequeando, espera!");
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/recover", new User
                {
                    Email = email.Encriptar(),
                });
                if (response.Status == "Ok")
                {
                    var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));

                    audioPlayer.Play();
                    //await Shell.Current.CurrentPage.DisplayAlert("Recuperación", response.Mensaje, "Continuar");
                    _ = Shell.Current.Navigation.PopToRootAsync();
                }
                else
                {
                    var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));

                    audioPlayer.Play();
                    _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Tus credenciales son incorrectas!");
                }
            }
            catch (Exception)
            {
                //await Shell.Current.CurrentPage.DisplayToastAsync("Lo sentimos, no pudimos contactar el servicio.");
                var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));

                audioPlayer.Play();
                //UpdatebyLucky
              //  CrossSimpleAudioPlayer.Current.Play();
                _ = Locator.MostrarPopupGenerico("Algo salio mal", "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.", "sadface.json", "Continuar");
            }
        }
    }
}
