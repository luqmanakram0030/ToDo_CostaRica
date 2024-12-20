using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using System;
using System.Threading.Tasks;
using ToDo_CostaRica.Infrastructure;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Response;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Mopups.Services;
using Newtonsoft.Json;
using ToDoCR.SharedDomain.Databases.Local;

namespace ToDo_CostaRica.ViewModels.Login
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAudioManager _audioManager;

        [ObservableProperty]
        public string email;

        [ObservableProperty]
        public string password;

        [ObservableProperty]
        public string password2;

        [ObservableProperty]
        public bool terminos;

        public IAsyncRelayCommand RegisterCommand { get; }

        public RegisterViewModel(IAudioManager audioManager)
        {
            _audioManager = audioManager;

            RegisterCommand = new AsyncRelayCommand(OnRegisterClicked);
        }

        private async Task OnRegisterClicked()
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

                var toast = Toast.Make("¡La longitud de tu clave debe ser de 6 o más!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            if (Password != Password2)
            {
                await PlayErrorSound();

                var toast = Toast.Make("¡Las contraseñas no coinciden!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            if (!Terminos)
            {
                await PlayErrorSound();

                var toast = Toast.Make("Debes aceptar la política", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            try
            {
                var toast = Toast.Make("¡Registrando tu cuenta!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();

                Locator.Instance.User.Email = Email.Encriptar();
                Locator.Instance.User.Password = Password.Encriptar();
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/register", Locator.Instance.User);

                if (response.Status == "Ok")
                {
                    await PlaySuccessSound();
                    Locator.Instance.User = JsonConvert.DeserializeObject<User>(response.User.Desencriptar());
                    Locator.Instance.RestClient.SetAuthToken();
                    _ = Locator.GuardarUser(false);

                    MessagingCenter.Send<object>(this, "SetLoginStatus");
                    await Shell.Current.Navigation.PopToRootAsync();

                    await Task.Delay(1000);
                    var successToast = Toast.Make("¡Tu cuenta ha sido creada!", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                    await successToast.Show();
                }
                else
                {
                    await PlayErrorSound();
                    var errorToast = Toast.Make(response.Mensaje, CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                    await errorToast.Show();
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