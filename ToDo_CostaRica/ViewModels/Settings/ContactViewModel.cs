using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Settings
{
    public class ContactViewModel : ViewModelBase
    {
        string email;
        public AsyncCommand LoginCommand { get; }
        public AsyncCommand CerrarCommand { get; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public ContactViewModel()
        {
            LoginCommand = new AsyncCommand(OnLoginClicked, allowsMultipleExecutions: false);
            CerrarCommand = new AsyncCommand(CerrarClicked, allowsMultipleExecutions: false);
            CrossSimpleAudioPlayer.Current.Load("error.wav");
        }
        
        async Task CerrarClicked()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async Task OnLoginClicked()
        {
            if (!email.EsEmail())
            {
                CrossSimpleAudioPlayer.Current.Play();
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
                    await PopupNavigation.Instance.PushAsync(new MailSentPopup());
                    //await Shell.Current.CurrentPage.DisplayAlert("Recuperación", response.Mensaje, "Continuar");
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
