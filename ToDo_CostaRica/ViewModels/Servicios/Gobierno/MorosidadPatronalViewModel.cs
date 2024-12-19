using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Models.Store;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Models;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Servicios.Gobierno
{
    public class MorosidadPatronalViewModel : ViewModelBase, IHeaderServicio, IConsultaServicio
    {
        HeaderServicioEnum headerServicioEnum;
        MorosidadPatronalModel morosidadPatronalModel;
        string cedula;

        public MorosidadPatronalViewModel()
        {
            HeaderServicioEnum = HeaderServicioEnum.Formulario;
            Title = "Morosidad Patronal";

            EjecutarCommand = new AsyncCommand(EjecutarBusquedaClickAsync, allowsMultipleExecutions: false);
            HistorialBusqueda = new ObservableRangeCollection<SearchPersona<MorosidadPatronalModel>>();

            GoToFormularioCommand = new Command(GoTo_Formulario);
            GoToHistorialCommand = new Command(GoTo_Historial);
            GoToInfoCommand = new Command(GoTo_Informacion);
            //SearchCommand = new Command(Search);
            CerrarCommand = new AsyncCommand(Cerrar);
            MasAccionesCommand = new AsyncCommand(MasAcciones);
            ConsultarNuevaPersonaCommand = new AsyncCommand<string>(ConsultarNuevaPersona);
            Locator.GetInstance().TodoCRAds.CargarInterstitial();
            BlobCache.LocalMachine
                .GetObject<List<SearchPersona<MorosidadPatronalModel>>>("SearchPersonaCCSS")
                .Subscribe(x => HistorialBusqueda.AddRange(x.OrderByDescending(p => p.FechaTicks)));
            LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
        }

        public string Cedula
        {
            get => cedula;
            set => SetProperty(ref cedula, value);
        }
        public ICommand GoToFormularioCommand { get; }
        public ICommand GoToHistorialCommand { get; }
        public ICommand GoToInfoCommand { get; }
        public ICommand CerrarCommand { get; }
        public ICommand ConsultarNuevaPersonaCommand { get; }
        public HeaderServicioEnum HeaderServicioEnum
        {
            get => headerServicioEnum;
            set => SetProperty(ref headerServicioEnum, value);
        }
        public ICommand MasAccionesCommand { get; }
        public MorosidadPatronalModel Response { get => morosidadPatronalModel; set => SetProperty(ref morosidadPatronalModel, value); }
        public ObservableRangeCollection<SearchPersona<MorosidadPatronalModel>> HistorialBusqueda { get; }

        async Task EjecutarBusquedaClickAsync()
        {
            if (!Cedula.EsCedulaValida())
            {
                await Shell.Current.CurrentPage.DisplayToastAsync("Esa identificación no es válida");
                return;
            }

            LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();

            HeaderServicioEnum = HeaderServicioEnum.Buscando;

            await Task.Delay(1000);
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();

            try
            {
                Response = await Locator.Instance.RestClient.PostAsync<MorosidadPatronalModel>("/persona/morosidadpatronal", new { Dato = cedula?.Encriptar(), Tipo = cedula != null ? 1 : 2 });
            }
            catch (Exception)
            {
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                Response = new MorosidadPatronalModel() { Status = "Error" };
            }

            /// Esperar para que se cierre el anuncio y continuar con la logica
            await Locator.GetInstance().TodoCRAds.EsperarAnuncio();

            if (Response?.Status == "OK")
            {
                _ = Task.Run(() =>
                {
                    var searchLogItem = new SearchPersona<MorosidadPatronalModel>()
                    {
                        Cedula = cedula,
                        Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm tt"),
                        FechaTicks = DateTime.Now.Ticks,
                        Nombre = $"{Response?.Nombre}"
                    };

                    HistorialBusqueda.Insert(0, searchLogItem);
                    if (HistorialBusqueda.Count > 20)
                    {
                        HistorialBusqueda.RemoveAt(HistorialBusqueda.Count - 1);
                    }
                    BlobCache.LocalMachine.InsertObject<List<SearchPersona<MorosidadPatronalModel>>>("SearchPersonaCCSS", HistorialBusqueda.ToList());
                });

                HeaderServicioEnum = HeaderServicioEnum.Resultados;
            }
            else
            {
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                await Shell.Current.CurrentPage.DisplaySnackBarAsync(Response?.Mensaje ?? DefaultMessages.ServiceError, "OK", null);
            }
        }

        /// <summary>
        /// //false is default value when system call back press
        /// </summary>
        /// <returns></returns>
        public virtual bool OnBackButtonPressed()
        {
            if (HeaderServicioEnum != HeaderServicioEnum.Formulario)
            {
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                Cedula = null;
                return false;
            }
            return true;
        }

        async Task ConsultarNuevaPersona(string ced)
        {
            if (ced.EsCedulaValida() || ced.EsSociedadValida() || ced.EsDIMEX())
            {
                Cedula = ced;
                Response = null;
                HeaderServicioEnum = HeaderServicioEnum.Formulario;
                await Shell.Current.CurrentPage.DisplayToastAsync("Presiona CONTINUAR!", 1000);
            }
            else
            {
                await Shell.Current.CurrentPage.DisplayToastAsync("Esa identificación no es válida");
            }
        }

        async Task MasAcciones()
        {
            var elementos = new List<string>() { "Guardar consulta offline" };
            if (cedula.EsCedulaValida())
            {
            }
            var result = await Shell.Current.CurrentPage.DisplayActionSheet("Más acciones", "Cerrar", null, elementos.ToArray());
            switch (result)
            {
                case "Cancel":

                    break;

                case "Guardar consulta offline":
                    var item = HistorialBusqueda.FirstOrDefault(p => p.Cedula == Cedula);
                    var index = HistorialBusqueda.IndexOf(item);
                    item.Response = Response;
                    HistorialBusqueda[index] = item;
                    BlobCache.LocalMachine.InsertObject<List<SearchPersona<MorosidadPatronalModel>>>("SearchPersonaCCSS", HistorialBusqueda.ToList());
                    await Shell.Current.CurrentPage.DisplayToastAsync("Consulta disponible offline");
                    break;

                case "Consultar morosidad patronal":

                    break;
            }
        }

        void GoTo_Formulario()
        {
            HeaderServicioEnum = HeaderServicioEnum.Formulario;
        }

        void GoTo_Historial()
        {
            HeaderServicioEnum = HeaderServicioEnum.Historial;
        }

        void GoTo_Informacion()
        {
            HeaderServicioEnum = HeaderServicioEnum.Informacion;
        }

        async Task Cerrar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
