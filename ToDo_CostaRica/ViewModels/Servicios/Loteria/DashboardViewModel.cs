using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Mopups.Services;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Views.Servicios.Loteria;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Models;

using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.ViewModels.Servicios.Loteria
{
    [QueryProperty(nameof(Tipo), nameof(tipo))]
    [QueryProperty(nameof(Img), nameof(img))]
    
    public partial class DashboardViewModel : ObservableObject, IHeaderServicio, IConsultaServicio
    {
        bool literalAdVisible;
        
        public bool LiteralAdVisible
        {
            get => literalAdVisible;
            set => SetProperty(ref literalAdVisible, value);
        }

        public ICommand EjecutarCommand { get; set; }
        HeaderServicioEnum headerServicioEnum;
        string tipo;
        string img;
        int serie;
        int numero;
        int fracciones;
        int formulario;
        bool activarNotificaciones;
        bool init;
        bool mostrarActivarNotificaciones;
        public int Formulario
        {
            get => formulario;
            set
            {
                SetProperty(ref formulario, value);
            }
        }

        public int Serie
        {
            get => serie;
            set
            {
                SetProperty(ref serie, value);
            }
        }
        public int Numero
        {
            get => numero;
            set
            {
                SetProperty(ref numero, value);
            }
        }
        public int Fracciones
        {
            get => fracciones;
            set
            {
                SetProperty(ref fracciones, value);
            }
        }
        public string Tipo
        {
            get => tipo;
            set
            {
                tipo = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref tipo, value);
                //Title = tipo;
                if (tipo == "loterianacional")
                    ActivarNotificaciones = Locator.Instance.User.Config.PushLoteriaNacional;
                else if (tipo == "lotto")
                    ActivarNotificaciones = Locator.Instance.User.Config.PushLotto;
                else if (tipo == "chances")
                    ActivarNotificaciones = Locator.Instance.User.Config.PushChances;
                else if (tipo == "nuevostiempos")
                    ActivarNotificaciones = Locator.Instance.User.Config.PushNuevosTiempos;
                else if (tipo == "Tres_Monazos")
                    ActivarNotificaciones = Locator.Instance.User.Config.PushTresMonazos;
                MostrarActivarNotificaciones = !activarNotificaciones;
                _ = InitPage();
            }
        }

        public string Img
        {
            get => img;
            set
            {
                img = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref img, value);
            }
        }
        public ICommand GoToFormularioCommand { get; set; }

        public ICommand GoToHistorialCommand { get; set; }

        public ICommand GoToInfoCommand { get; set; }

        public ICommand CerrarCommand { get; set; }

        public HeaderServicioEnum HeaderServicioEnum
        {
            get => headerServicioEnum;
            set => SetProperty(ref headerServicioEnum, value);
        }

        public ICommand ConsultarPremiosCommand { get; set; }
        public ICommand MasAccionesCommand => throw new NotImplementedException();

        JPSBaseModel response;
        public JPSBaseModel Response { get => response; set => SetProperty(ref response, value); }
        public bool ActivarNotificaciones
        {
            get => activarNotificaciones;
            set
            {
                activarNotificaciones = SetProperty(ref activarNotificaciones, value);
                if (!init)
                {
                    if (tipo == "loterianacional")
                        Locator.Instance.User.Config.PushLoteriaNacional = value;
                    else if (tipo == "lotto")
                        Locator.Instance.User.Config.PushLotto = value;
                    else if (tipo == "chances")
                        Locator.Instance.User.Config.PushChances = value;
                    else if (tipo == "nuevostiempos")
                        Locator.Instance.User.Config.PushNuevosTiempos = value;
                    else if (tipo == "Tres_Monazos")
                        Locator.Instance.User.Config.PushTresMonazos = value;
                    _ = Locator.GuardarUser();
                    SetNotificaciones(value);
                }
            }
        }

        public bool MostrarActivarNotificaciones
        {
            get => mostrarActivarNotificaciones;
            set => SetProperty(ref mostrarActivarNotificaciones, value);
        }

        private async void SetNotificaciones(bool value)
        {
            _ = Toast.Make("Notificaciones " + (value ? "activadas" : "desactivadas"), ToastDuration.Short).Show();
            
        }

        public DashboardViewModel()
        {
            init = true;
            Fracciones = 1;
            Formulario = -2;
            //HeaderServicioEnum = HeaderServicioEnum.Buscando;
            CerrarCommand = new AsyncRelayCommand(Cerrar);
            ConsultarPremiosCommand = new AsyncRelayCommand(ConsultarPremios);
        }

        async Task Cerrar()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            await Shell.Current.GoToAsync("..");
        }

        public virtual bool OnBackButtonPressed()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            return true;
        }

        async Task ConsultarPremios()
        {
            try
            {
                if (serie < 0 || serie > 999)
                {
                    await Toast.Make("La serie debe ser entre 0 y 999", ToastDuration.Short).Show();
                    return;
                }
                if (numero < 0 || numero > 99)
                {
                    
                    await Toast.Make("El número debe ser entre 0 y 99", ToastDuration.Short).Show();
                    return;
                }

                var tipoJuego = "N";
                if (Tipo == "Chances")
                {
                    tipoJuego = "P";
                }

                var sorteo = Response?.LoteriaNacional?.FirstOrDefault()?.NumeroSorteo ?? Response?.Chances?.FirstOrDefault()?.NumeroSorteo ?? 0;
                DevuelvePremiosResponseAPIPremios response = await Locator.Instance.RestClient.PostAsync<DevuelvePremiosResponseAPIPremios>("/jps/sorteo", new { TipoLoteria = tipoJuego.Encriptar(), NumeroSorteo = sorteo, serie = serie, numero = numero });
                await MopupService.Instance.PushAsync(new SorteoPopup(response, sorteo, Serie, Numero, Fracciones));
            }
            catch (Exception)
            {
                
                await Shell.Current.CurrentPage.DisplayAlert("Un problema ha sucedido al obtener la información", "OK", null);
            }
        }

        async Task InitPage()
        {
            try
            {
                await Task.Delay(500);
                Formulario = 0;
                JObject keyValuePairs = new JObject
                {
                    { tipo.Replace(" ","").Replace("_",""), "last".Encriptar() }
                };
                Response = await Locator.Instance.RestClient.PostAsync<JPSBaseModel>("/jps/obtener", keyValuePairs);
                Formulario = Response.FormularioId;
                OnPropertyChanged("Img");
                Locator.GetInstance().TodoCRAds.CargarInterstitial();
                LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
            }
            catch (Exception)
            {
                Formulario = -1;
            }
            init = false;
        }
    }
}
