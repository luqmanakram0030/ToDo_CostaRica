using Akavache;
using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo_CostaRica.Infrastructure;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Models.Loteria;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        string email;
        string password;
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

        public LoginViewModel()
        {
            LoginCommand = new AsyncCommand(OnLoginClicked, allowsMultipleExecutions: false);
            CrossSimpleAudioPlayer.Current.Load("error.wav");
        }

        async Task OnLoginClicked()
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
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡La longitud de tu clave debe ser al menos 6!");
                return;
            }

            try
            {
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Chequeando tu cuenta!");
                var response = await Locator.Instance.RestClient.PostAsync<RpLogin>("/user/login", new User
                {
                    Email = email.Encriptar(),
                    Password = password.Encriptar(),
                    PlayerId = Locator.Instance.User.PlayerId,
                    PushToken = Locator.Instance.User.PushToken,
                });
                if (response.Status == "Ok")
                {
                    Locator.Instance.User = JsonConvert.DeserializeObject<User>(response.User.Desencriptar());
                    Locator.Instance.RestClient.SetAuthToken();
                    _ = Locator.GuardarUser(false);

                    BlobCache.LocalMachine.InsertObject<List<MiLoteriaGuardada>>("MisLoterias", response.Data.MiLoteriaGuardada);

                    MessagingCenter.Send<object>(this, "SetLoginUser");

                    _ = Shell.Current.Navigation.PopToRootAsync();
                }
                else
                {
                    CrossSimpleAudioPlayer.Current.Play();
                    _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Tus credenciales son incorrectas!");
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
