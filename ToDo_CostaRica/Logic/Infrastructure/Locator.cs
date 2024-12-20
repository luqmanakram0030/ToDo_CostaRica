using Akavache;

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Mopups.Services;
using ToDo_CostaRica.Services;
using ToDo_CostaRica.ViewModels;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain.Databases.Local;

namespace ToDo_CostaRica.Infrastructure
{
    public class Locator : IDisposable
    {
        public static Locator Instance;
        private RestClient restClient;

        public RestClient RestClient
        {
            get
            {
                if (restClient == null) restClient = new RestClient();
                return restClient;
            }
            set => restClient = value;
        }

        public User User { get; set; }
        public TodoCRAds TodoCRAds { get; }
        public bool OneSignalGet { get; internal set; }

        public Locator()
        {
            Instance = this;
            TodoCRAds = new TodoCRAds();
            SetLoadingModalPreferences();
        }

        public static async Task GuardarUser(bool syncServer = true)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    if (syncServer)
                    {
                        dynamic response = await Locator.Instance.RestClient.PostAsync<User>("/user/auth", Locator.Instance.User);
                        if (!(Locator.Instance.User.Id > 0))
                        {
                            Locator.Instance.User.Id = response.Id;
                        }
                    }
                    await BlobCache.Secure.InsertObject<User>("User", Locator.Instance.User);
                }
                catch (Exception ex)
                {
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                    try
                    {
                        await BlobCache.Secure.InvalidateObject<User>("User");
                        await BlobCache.Secure.InsertObject<User>("User", Locator.Instance.User);
                    }
                    catch (Exception ex2)
                    {
                        Microsoft.AppCenter.Crashes.Crashes.TrackError(ex2);
                    }
                }
            });
        }

        public static async Task MostrarOneTimePopup(string titulo, string mensaje, string imagen, string boton, string llave)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                try
                {
                    bool? x = null;
                    x = await BlobCache.LocalMachine.GetObject<bool>(llave);
                    if (x != true)
                    {
                        await MopupService.Instance.PushAsync(new CustomPopupMessage(new CustomPopupMessageViewModel(titulo, mensaje, imagen, boton)));
                        await BlobCache.LocalMachine.InsertObject<bool>(llave, true);
                    }
                }
                catch (Exception)
                {
                    await MopupService.Instance.PushAsync(new CustomPopupMessage(new CustomPopupMessageViewModel(titulo, mensaje, imagen, boton)));
                    await BlobCache.LocalMachine.InsertObject<bool>(llave, true);
                }
            });
        }

        public static async Task MostrarPopupGenerico(string titulo, string mensaje, string imagen, string boton, int? delay = null)
        {
            await Task.Run(async () =>
            {
                if (!delay.HasValue)
                    await Task.Delay(1000);
                await MopupService.Instance.PushAsync(new CustomPopupMessage(new CustomPopupMessageViewModel(titulo, mensaje, imagen, boton)));
            });
        }

        public static Locator GetInstance()
        {
            if (Instance == null)
                return new Locator();

            return Instance;
        }

        public static void SetLoadingModalPreferences(string defaultMessage = "Cargando...")
        {
            AiForms.Dialogs.Configurations.LoadingConfig = new AiForms.Dialogs.LoadingConfig
            {
                IndicatorColor = Colors.White,
                OverlayColor = Colors.Black,
                Opacity = 0.9,
                DefaultMessage = defaultMessage,
            };
            
        }

        public void Dispose()
        {
            TodoCRAds.Dispose();
        }
    }
}