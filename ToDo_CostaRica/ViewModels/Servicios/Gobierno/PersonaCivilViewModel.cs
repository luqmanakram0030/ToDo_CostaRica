using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Models.Store;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Servicios.Gobierno
{
    public class PersonaCivilViewModel : ViewModelBase, IHeaderServicio, IConsultaServicio
    {
        string cedula;
        string nombre;
        HeaderServicioEnum headerServicioEnum;
        RpTSE response;

        public PersonaCivilViewModel()
        {
            HeaderServicioEnum = HeaderServicioEnum.Formulario;
            Title = "Consulta de persona";
            EjecutarCommand = new AsyncCommand(EjecutarBusquedaClickAsync, allowsMultipleExecutions: false);
            GoToFormularioCommand = new Command(GoTo_Formulario);
            GoToHistorialCommand = new Command(GoTo_Historial);
            GoToInfoCommand = new Command(GoTo_Informacion);
            //SearchCommand = new Command(Search);
            CerrarCommand = new AsyncCommand(Cerrar);
            MasAccionesCommand = new AsyncCommand(MasAcciones);
            ConsultarNuevaPersonaCommand = new AsyncCommand<string>(ConsultarNuevaPersona);
            HistorialBusqueda = new ObservableRangeCollection<SearchPersona<RpTSE>>();
            Locator.GetInstance().TodoCRAds.CargarInterstitial();
            BlobCache.LocalMachine.GetObject<List<SearchPersona<RpTSE>>>("SearchPersonaCivil").Subscribe(x => HistorialBusqueda.AddRange(x.OrderByDescending(p => p.FechaTicks)));
            LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
            //_ = Task.Run(() =>
            //{
            //    // Para tener todo a la mano y montarlo rapido
            //    var dato = "{\"status\":\"OK\",\"mensaje\":null,\"sociedad\":null,\"civil\":{\"cedula\":\"503660905\",\"nombre\":\"JENIFER ANDREA\",\"apellido1\":\"JIMENEZ\",\"apellido2\":\"ZUÑIGA\",\"conocidoComo\":\"\",\"fechaNacimiento\":\"20/11/1988\",\"lugarNacimiento\":\"CENTRO NICOYA GUANACASTE\",\"nacionalidad\":\"COSTARRICENSE\",\"genero\":\"FEMENINO\",\"nombrePadre\":\"FRANKLIN JIMENEZ ROJAS\",\"identificacionPadre\":\"203070722\",\"nombreMadre\":\"TERESA ZUÑIGA MATARRITA\",\"identificacionMadre\":\"501690123\",\"empadronado\":true,\"fallecido\":false,\"marginal\":false,\"edad\":null,\"matrimonios\":[],\"hijos\":[{\"cedula\":\"121280894\",\"nombre\":\"AMANDA SOFIA\",\"apellido1\":\"MATARRITA\",\"apellido2\":\"JIMENEZ\",\"conocidoComo\":\"\",\"fechaNacimiento\":\"07/11/2011\",\"lugarNacimiento\":\"CARMEN CENTRAL SAN JOSE\",\"nacionalidad\":\"COSTARRICENSE\",\"genero\":\"FEMENINO\",\"nombrePadre\":\"JORGE EDUARDO MATARRITA RAMIREZ\",\"identificacionPadre\":\"603730575\",\"nombreMadre\":\"JENIFER ANDREA JIMENEZ ZUÑIGA\",\"identificacionMadre\":\"503660905\",\"empadronado\":false,\"fallecido\":false,\"marginal\":false,\"edad\":null,\"matrimonios\":null,\"hijos\":null,\"lugarVotacion\":null,\"defuncion\":null},{\"cedula\":\"123480814\",\"nombre\":\"EMMA LUCIA\",\"apellido1\":\"MATARRITA\",\"apellido2\":\"JIMENEZ\",\"conocidoComo\":\"\",\"fechaNacimiento\":\"11/03/2020\",\"lugarNacimiento\":\"CARMEN CENTRAL SAN JOSE\",\"nacionalidad\":\"COSTARRICENSE\",\"genero\":\"FEMENINO\",\"nombrePadre\":\"JORGE EDUARDO MATARRITA RAMIREZ\",\"identificacionPadre\":\"603730575\",\"nombreMadre\":\"JENIFER ANDREA JIMENEZ ZUÑIGA\",\"identificacionMadre\":\"503660905\",\"empadronado\":false,\"fallecido\":false,\"marginal\":false,\"edad\":null,\"matrimonios\":null,\"hijos\":null,\"lugarVotacion\":null,\"defuncion\":null}],\"lugarVotacion\":{\"provincia\":\"SAN JOSE\",\"canton\":\"GOICOECHEA\",\"distritoAdministrativo\":\"GUADALUPE\",\"distritoElectoral\":\"GUADALUPE\",\"numeroElector\":\"* POR DEFINIR *\",\"centroVotacion\":\"* POR DEFINIR *\",\"numeroJunta\":\"* POR DEFINIR *\",\"fechaVencimientoCedula\":\"06/11/2028\",\"inscritoCantonDesde\":\"06/11/2018\",\"inscritoDistritoDesde\":\"06/11/2018\"},\"defuncion\":null},\"dimex\":null}";
            //    Response = Newtonsoft.Json.JsonConvert.DeserializeObject<RpTSE>(dato);
            //    Cedula = response.Civil.Cedula;
            //    HeaderServicioEnum = HeaderServicioEnum.Resultados;
            //});
        }

        public ObservableRangeCollection<SearchPersona<RpTSE>> HistorialBusqueda { get; }
        public ICommand GoToFormularioCommand { get; }
        public ICommand GoToHistorialCommand { get; }
        public ICommand GoToInfoCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CerrarCommand { get; }
        public ICommand MasAccionesCommand { get; }
        public ICommand ConsultarNuevaPersonaCommand { get; }
        public HeaderServicioEnum HeaderServicioEnum
        {
            get => headerServicioEnum;
            set => SetProperty(ref headerServicioEnum, value);
        }

        public RpTSE Response
        {
            get => response;
            set => SetProperty(ref response, value);
        }

        public string Cedula
        {
            get => cedula;
            set
            {
                if (!string.IsNullOrEmpty(Nombre))
                {
                    Nombre = null;
                }
                SetProperty(ref cedula, value);
            }
        }
        public string Nombre
        {
            get => nombre;
            set
            {
                if (!string.IsNullOrEmpty(Cedula))
                {
                    Cedula = null;
                }
                SetProperty(ref nombre, value);
            }
        }

        async Task EjecutarBusquedaClickAsync()
        {
            if (string.IsNullOrEmpty(Nombre) && !Cedula.EsCedulaValida())
            {
                await Shell.Current.CurrentPage.DisplayToastAsync("Esa identificación no es válida");
                return;
            }
            else if (!string.IsNullOrEmpty(Nombre) && Nombre.Split(' ').Length < 2)
            {
                await Shell.Current.CurrentPage.DisplayToastAsync("Debes escribir al menos dos palabras");
                return;
            }

            LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();

            if (!string.IsNullOrEmpty(cedula) && (response?.Civil?.Cedula == cedula || response?.Sociedad?.Results?.FirstOrDefault()?.Cedula == cedula))
            {
                HeaderServicioEnum = HeaderServicioEnum.Resultados;
            }
            else
            {
                HeaderServicioEnum = HeaderServicioEnum.Buscando;

                await Task.Delay(1000);
                Locator.GetInstance().TodoCRAds.MostrarInterstitial();

                try
                {
                    Response = await Locator.Instance.RestClient.PostAsync<RpTSE>("/tse/getdato", new { Dato = cedula?.Encriptar() ?? nombre.Encriptar(), Tipo = cedula != null ? 1 : 2 });
                }
                catch (Exception)
                {
                    HeaderServicioEnum = HeaderServicioEnum.Formulario;
                    Response = new RpTSE() { Status = "Error" };
                }

                /// Esperar para que se cierre el anuncio y continuar con la logica
                await Locator.GetInstance().TodoCRAds.EsperarAnuncio();

                if (Response.Status == "OK")
                {
                    if (Response.Civil != null)
                    {
                        _ = Task.Run(() =>
                        {
                            var searchLogItem = new SearchPersona<RpTSE>()
                            {
                                Cedula = cedula,
                                Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm tt"),
                                FechaTicks = DateTime.Now.Ticks,
                                Nombre = $"{Response?.Civil?.Nombre} {Response?.Civil?.Apellido1} {Response?.Civil?.Apellido2}"
                            };
                            HistorialBusqueda.Insert(0, searchLogItem);
                            if (HistorialBusqueda.Count > 20)
                            {
                                HistorialBusqueda.RemoveAt(HistorialBusqueda.Count - 1);
                            }
                            BlobCache.LocalMachine.InsertObject<List<SearchPersona<RpTSE>>>("SearchPersonaCivil", HistorialBusqueda.ToList());
                        });

                        MessagingCenter.Send<object>(this, "ScrollTo");

                        HeaderServicioEnum = HeaderServicioEnum.Resultados;
                    }
                    else if (Response.ListaPersonas != null)
                    {
                        HeaderServicioEnum = HeaderServicioEnum.ListaResultados;
                    }
                }
                else
                {
                    HeaderServicioEnum = HeaderServicioEnum.Formulario;
                    await Shell.Current.CurrentPage.DisplaySnackBarAsync(Response.Mensaje ?? DefaultMessages.ServiceError, "OK", null);
                }
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
                elementos.Add("Consultar morosidad patronal");
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
                    BlobCache.LocalMachine.InsertObject<List<SearchPersona<RpTSE>>>("SearchPersonaCivil", HistorialBusqueda.ToList());
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
