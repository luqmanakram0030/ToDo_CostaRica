
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Google.MobileAds;
using Microsoft.Maui.Controls;
using Mopups.Services;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDo_CostaRica.Views.Login;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;


namespace ToDo_CostaRica.ViewModels.Settings
{
    public class ContactViewModel : ViewModelBase
    {
        string email;
        public ICommand LoginCommand { get; }
        public IAsyncRelayCommand CerrarCommand { get; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public ContactViewModel(IAudioManager _audioManager)
        {
            this._audioManager = _audioManager;
            LoginCommand = new AsyncRelayCommand(OnLoginClicked);
            CerrarCommand = new AsyncRelayCommand(CerrarClicked);
           
        }
        private async Task PlayErrorSound()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));
            player.Play();
        }
        async Task CerrarClicked()
        {
            await MopupService.Instance.PopAsync();
        }
        private readonly IAudioManager _audioManager;
        async Task OnLoginClicked()
        {
            if (!email.EsEmail())
            {
               await PlayErrorSound();
               
                _ = Toast.Make("¡Necesitas un email válido!", ToastDuration.Short).Show();
                return;
            }

            try
            {
                
                _ = Toast.Make("¡Chequeando, espera!", ToastDuration.Short).Show();
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/recover", new User
                {
                    Email = email.Encriptar(),
                });
                if (response.Status == "Ok")
                {
                    await MopupService.Instance.PushAsync(new MailSentPopup());
                    //await Shell.Current.CurrentPage.DisplayAlert("Recuperación", response.Mensaje, "Continuar");
                    _ = Shell.Current.Navigation.PopToRootAsync();
                }
                else
                {
                    await PlayErrorSound();
                    _ = Toast.Make("¡Tus credenciales son incorrectas!", ToastDuration.Short).Show();
                   
                }
            }
            catch (Exception)
            {
                //await Shell.Current.CurrentPage.DisplayToastAsync("Lo sentimos, no pudimos contactar el servicio.");
                await PlayErrorSound();
                _ = Locator.MostrarPopupGenerico("Algo salio mal", "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.", "sadface.json", "Continuar");
            }
        }
    }
}
