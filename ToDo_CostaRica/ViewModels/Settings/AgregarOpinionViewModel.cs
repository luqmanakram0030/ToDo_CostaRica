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
    public class AgregarOpinionViewModel : ViewModelBase
    {
        string comentario;
        public AsyncCommand AgregarCommand { get; }
        public AsyncCommand CerrarCommand { get; }

        public string Comentario
        {
            get => comentario;
            set => SetProperty(ref comentario, value);
        }

        public AgregarOpinionViewModel()
        {
            AgregarCommand = new AsyncCommand(Agregar, allowsMultipleExecutions: false);
            CerrarCommand = new AsyncCommand(CerrarClicked, allowsMultipleExecutions: false);
            CrossSimpleAudioPlayer.Current.Load("error.wav");
        }

        async Task CerrarClicked()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async Task Agregar()
        {
            if (string.IsNullOrEmpty(comentario))
            {
                CrossSimpleAudioPlayer.Current.Play();
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Escribe tu comentario!");
                return;
            }
            IsBusy = true;
            try
            {
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Guardando, espera!");
                RpComment response = await Locator.Instance.RestClient.PostAsync<RpComment>("/user/add-comment", new
                {
                    Comment = comentario,
                });
                if (response.Status == "OK")
                {
                    await Shell.Current.CurrentPage.DisplayAlert("¡Recibido!", "Ya lo tenemos guardado, te responderemos pronto.", "Continuar");
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    CrossSimpleAudioPlayer.Current.Play();
                    _ = Shell.Current.CurrentPage.DisplayToastAsync("Intenta de nuevo, no lo logramos.");
                }
            }
            catch (Exception)
            {
                //await Shell.Current.CurrentPage.DisplayToastAsync("Lo sentimos, no pudimos contactar el servicio.");
                CrossSimpleAudioPlayer.Current.Play();
                _ = Locator.MostrarPopupGenerico("Algo salio mal", "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.", "sadface.json", "Continuar");
            }
            IsBusy = false;
        }
    }
}
