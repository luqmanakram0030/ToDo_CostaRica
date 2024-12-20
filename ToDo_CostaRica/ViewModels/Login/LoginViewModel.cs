using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Mopups.Services;
using Newtonsoft.Json;
using ToDoCR.SharedDomain.Models.Loteria;

namespace ToDo_CostaRica.ViewModels.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAudioManager _audioManager;

        [ObservableProperty]
        public string email;

        [ObservableProperty]
        public string password;

        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel(IAudioManager audioManager)
        {
            _audioManager = audioManager;

            LoginCommand = new AsyncRelayCommand(OnLoginClicked);
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

            if (string.IsNullOrEmpty(Password) || Password.Length <= 6)
            {
                await PlayErrorSound();

                var toast = Toast.Make("¡La longitud de tu clave debe ser al menos 6!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            try
            {
                var toast = Toast.Make("¡Chequeando tu cuenta!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();

                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/login", new User
                {
                    Email = Email.Encriptar(),
                    Password = Password.Encriptar(),
                    PlayerId = Locator.Instance.User.PlayerId,
                    PushToken = Locator.Instance.User.PushToken,
                });

                if (response.Status == "Ok")
                {
                    await PlaySuccessSound();
                    Locator.Instance.User = JsonConvert.DeserializeObject<User>(response.User.Desencriptar());
                    Locator.Instance.RestClient.SetAuthToken();
                    _ = Locator.GuardarUser(false);

                    BlobCache.LocalMachine.InsertObject<List<MiLoteriaGuardada>>("MisLoterias", response.Data.MiLoteriaGuardada);

                    MessagingCenter.Send<object>(this, "SetLoginUser");

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