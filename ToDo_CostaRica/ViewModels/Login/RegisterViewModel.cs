using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo_CostaRica.Infrastructure;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Login
{
    public class RegisterViewModel : ViewModelBase
    {
        string email;
        string password;
        string password2;
        bool terminos;
        public AsyncCommand LoginCommand { get; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Password2
        {
            get => password2;
            set => SetProperty(ref password2, value);
        }

        public bool Terminos
        {
            get => terminos;
            set => SetProperty(ref terminos, value);
        }

        public RegisterViewModel()
        {
            LoginCommand = new AsyncCommand(OnRegisterClicked, allowsMultipleExecutions: false);
            CrossSimpleAudioPlayer.Current.Load("error.wav");
        }

        async Task OnRegisterClicked()
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            //var page = Shell.Current.CurrentPage;
            //Shell.Current.Navigation.RemovePage(page);
            //await Shell.Current.GoToAsync("HomePage");

            if (!email.EsEmail())
            {
                CrossSimpleAudioPlayer.Current.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Necesitas un email válido!");
                return;
            }

            if (string.IsNullOrEmpty(password) || password?.Length <= 6)
            {
                CrossSimpleAudioPlayer.Current.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡La longitud de tu clave debe ser de 6 o más!");
                return;
            }

            if (password != password2)
            {
                CrossSimpleAudioPlayer.Current.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Las contraseñas no coinciden!");
                return;
            }

            if (terminos)
            {
                CrossSimpleAudioPlayer.Current.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("Debes aceptar la política");
                return;
            }

            try
            {
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Registrando tu cuenta!");
                Locator.Instance.User.Email = email.Encriptar();
                Locator.Instance.User.Password = password.Encriptar();
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/register", Locator.Instance.User);

                if (response.Status == "Ok")
                {
                    Locator.Instance.User = JsonConvert.DeserializeObject<User>(response.User.Desencriptar());
                    Locator.Instance.RestClient.SetAuthToken();
                    _ = Locator.GuardarUser(false);

                    MessagingCenter.Send<object>(this, "SetLoginStatus");
                    _ = Shell.Current.Navigation.PopToRootAsync();

                    await Task.Delay(1000);
                    _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Tu cuenta ha sido creada!");
                }
                else
                {
                    CrossSimpleAudioPlayer.Current.Play();
                    _ = Shell.Current.CurrentPage.DisplayToastAsync(response.Mensaje);
                }
            }
            catch (Exception)
            {
                //await Shell.Current.CurrentPage.DisplayToastAsync("Lo sentimos, no pudimos contactar el servicio.");
                CrossSimpleAudioPlayer.Current.Play();
                _ = Locator.MostrarPopupGenerico("Algo salio mal", "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.", "sadface.json", "Continuar");
            }
        }
    }
}
