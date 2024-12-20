using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Akavache;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Models.Store;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Models;

namespace ToDo_CostaRica.ViewModels.Servicios.Gobierno
{
    public partial class MorosidadPatronalViewModel : ObservableObject, IHeaderServicio, IConsultaServicio
    {
        private readonly IAudioManager _audioManager;

        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private HeaderServicioEnum headerServicioEnum;

        [ObservableProperty]
        private MorosidadPatronalModel response;

        [ObservableProperty]
        private bool literalAdVisible;

        public ObservableCollection<SearchPersona<MorosidadPatronalModel>> HistorialBusqueda { get; } = new();

        public IAsyncRelayCommand EjecutarCommand { get; }
        public IAsyncRelayCommand<string> ConsultarNuevaPersonaCommand { get; }
        public ICommand GoToFormularioCommand { get; }
        public ICommand GoToHistorialCommand { get; }
        public ICommand GoToInfoCommand { get; }
        public ICommand CerrarCommand { get; }
        public ICommand MasAccionesCommand { get; }

        public MorosidadPatronalViewModel(IAudioManager audioManager)
        {
            _audioManager = audioManager;

            HeaderServicioEnum = HeaderServicioEnum.Formulario;
            EjecutarCommand = new AsyncRelayCommand(EjecutarBusquedaClickAsync);
            ConsultarNuevaPersonaCommand = new AsyncRelayCommand<string>(ConsultarNuevaPersonaAsync);
            CerrarCommand = new AsyncRelayCommand(CerrarAsync);
            MasAccionesCommand = new AsyncRelayCommand(MasAccionesAsync);

            GoToFormularioCommand = new Command(() => HeaderServicioEnum = HeaderServicioEnum.Formulario);
            GoToHistorialCommand = new Command(() => HeaderServicioEnum = HeaderServicioEnum.Historial);
            GoToInfoCommand = new Command(() => HeaderServicioEnum = HeaderServicioEnum.Informacion);

            LoadHistorial();
        }

        private void LoadHistorial()
        {
            BlobCache.LocalMachine
                .GetObject<List<SearchPersona<MorosidadPatronalModel>>>("SearchPersonaCCSS")
                .Subscribe(x =>
                {
                    foreach (var item in x.OrderByDescending(p => p.FechaTicks))
                    {
                        HistorialBusqueda.Add(item);
                    }
                });
        }
        private async Task EjecutarBusquedaClickAsync()
        {
            if (!Cedula.EsCedulaValida())
            {
                await ShowToastAsync("Esa identificación no es válida");
                await PlayErrorSoundAsync();
                return;
            }

            LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
            HeaderServicioEnum = HeaderServicioEnum.Buscando;

            try
            {
                await Task.Delay(1000);
                Locator.GetInstance().TodoCRAds.MostrarInterstitial();

                Response = await Locator.Instance.RestClient.PostAsync<MorosidadPatronalModel>("/persona/morosidadpatronal", new { Dato = Cedula.Encriptar(), Tipo = 1 });

                if (Response?.Status == "OK")
                {
                    SaveToHistorial();
                    HeaderServicioEnum = HeaderServicioEnum.Resultados;
                }
                else
                {
                    await ShowErrorAsync(Response?.Mensaje ?? DefaultMessages.ServiceError);
                    HeaderServicioEnum = HeaderServicioEnum.Formulario;
                }
            }
            catch (Exception)
            {
                await ShowErrorAsync("Ocurrió un error inesperado, intenta nuevamente.");
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
            }
        }

        private void SaveToHistorial()
        {
            Task.Run(() =>
            {
                var searchLogItem = new SearchPersona<MorosidadPatronalModel>
                {
                    Cedula = Cedula,
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm tt"),
                    FechaTicks = DateTime.Now.Ticks,
                    Nombre = Response?.Nombre
                };

                HistorialBusqueda.Insert(0, searchLogItem);

                if (HistorialBusqueda.Count > 20)
                {
                    HistorialBusqueda.RemoveAt(HistorialBusqueda.Count - 1);
                }

                BlobCache.LocalMachine.InsertObject("SearchPersonaCCSS", HistorialBusqueda.ToList());
            });
        }

        private async Task ConsultarNuevaPersonaAsync(string ced)
        {
            if (ced.EsCedulaValida() || ced.EsSociedadValida() || ced.EsDIMEX())
            {
                Cedula = ced;
                Response = null;
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                await ShowToastAsync("Presiona CONTINUAR!");
            }
            else
            {
                await ShowToastAsync("Esa identificación no es válida");
                await PlayErrorSoundAsync();
            }
        }

        private async Task MasAccionesAsync()
        {
            var elementos = new List<string> { "Guardar consulta offline" };
            var result = await Shell.Current.CurrentPage.DisplayActionSheet("Más acciones", "Cerrar", null, elementos.ToArray());

            if (result == "Guardar consulta offline")
            {
                var item = HistorialBusqueda.FirstOrDefault(p => p.Cedula == Cedula);
                if (item != null)
                {
                    item.Response = Response;
                    BlobCache.LocalMachine.InsertObject("SearchPersonaCCSS", HistorialBusqueda.ToList());
                    await ShowToastAsync("Consulta disponible offline");
                }
            }
        }

        private async Task CerrarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task PlayErrorSoundAsync()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));
            player.Play();
        }

        private async Task ShowToastAsync(string message)
        {
            var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
            await toast.Show();
        }

        private async Task ShowErrorAsync(string message)
        {
            await ShowToastAsync(message);
            await PlayErrorSoundAsync();
        }

        public bool OnBackButtonPressed()
        {
            if (HeaderServicioEnum != HeaderServicioEnum.Formulario)
            {
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                Cedula = null;
                return false;
            }
            return true;
        }
    }
}
