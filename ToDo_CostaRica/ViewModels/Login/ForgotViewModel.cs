using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Mopups.Services;

namespace ToDo_CostaRica.ViewModels.Login
{
    public partial class ForgotViewModel : ObservableObject
    {
        private readonly IAudioManager _audioManager;
        

        [ObservableProperty]
        public string email;

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand CerrarCommand { get; }

        public ForgotViewModel(IAudioManager audioManager)
        {
            _audioManager = audioManager;

            LoginCommand = new AsyncRelayCommand(OnLoginClicked);
            CerrarCommand = new AsyncRelayCommand(CerrarClicked);
        }

        private async Task CerrarClicked()
        {
            // Using Mopup for popups in MAUI
            await MopupService.Instance.PopAsync();
        }

        private async Task OnLoginClicked()
        {
            if (!Email.EsEmail())
            {
                await PlayErrorSound();
               
                var toast = Toast.Make("¡Necesitas un email válido!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            try
            {
               
                var toast = Toast.Make("¡Chequeando, espera!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/recover", new User
                {
                    Email = Email.Encriptar(),
                });

                if (response.Status == "Ok")
                {
                    await PlaySuccessSound();
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                else
                {
                    await PlayErrorSound();
                    var toast1 = Toast.Make("¡Tus credenciales son incorrectas!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                    await toast1.Show();
                   
                }
            }
            catch (Exception)
            {
                await PlayErrorSound();
                await Locator.MostrarPopupGenerico(
                    "Algo salio mal",
                    "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.",
                    "sadface.json",
                    "Continuar");
            }
        }

        private async Task PlayErrorSound()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));
            player.Play();
        }

        private async Task PlaySuccessSound()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("success.wav"));
            player.Play();
        }
    }
}
