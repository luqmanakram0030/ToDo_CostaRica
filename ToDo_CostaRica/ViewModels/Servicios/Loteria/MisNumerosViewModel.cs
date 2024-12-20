using Akavache;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Views;
using ToDo_CostaRica.Views.Servicios.Loteria;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Models;
using ToDoCR.SharedDomain.Models.Loteria;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.ViewModels.Servicios.Loteria
{
    public class MisNumerosViewModel : ViewModelBase, IHeaderServicio, IConsultaServicio
    {
        private int _selectedViewModelIndex = 0;
        int formulario;
        int fracciones;
        HeaderServicioEnum headerServicioEnum;
        ObservableRangeCollection<MiLoteriaGuardada> listaMisLoterias;
        MiLoteriaGuardada miLoteria;
        int numero;
        int serie;
        string tipoJuego;
        public MisNumerosViewModel()
        {
            Title = "Mis números";
            Fracciones = 1;
            Formulario = 0;
            //HeaderServicioEnum = HeaderServicioEnum.Buscando;
            CerrarCommand = new AsyncCommand(Cerrar);
            AgregarCommand = new Command(Agregar);
            EliminarCommand = new AsyncCommand<MiLoteriaGuardada>(Eliminar, allowsMultipleExecutions: false);
            MiLoteria = new MiLoteriaGuardada();
            listaMisLoterias = new ObservableRangeCollection<MiLoteriaGuardada>();
            BlobCache.LocalMachine.GetObject<List<MiLoteriaGuardada>>("MisLoterias").Subscribe(x =>
            {
                listaMisLoterias.AddRange(x.OrderBy(p => p.Tipo));
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (listaMisLoterias.Any(p => p.Id == 0)) // No esta enviado al server
                    {
                        SyncData();
                    }
                    if (listaMisLoterias.Any(p => !p.Notificado.HasValue)) // Hay sorteos sin notificar, revisar la BD para sincronizar y ver si ya fue notificado
                    {
                        SyncData();
                    }
                }
            });
            _ = Locator.MostrarOneTimePopup("Esto es importante", "Para ser notificado no es necesario que hagas una cuenta," +
                " sin embargo, ten presente que si reinicias" +
                " la app tus números se pueden eliminar. Si quieres mantener la información," +
                " puedes crear una cuenta gratis.", "serviceschedule", "Entendido", "OneTimeMessageMiLoteria");
        }


        public ICommand AgregarCommand { get; set; }
        public ICommand CerrarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public int Formulario
        {
            get => formulario;
            set
            {
                SetProperty(ref formulario, value);
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

        public ICommand GoToFormularioCommand { get; set; }

        public ICommand GoToHistorialCommand { get; set; }

        public ICommand GoToInfoCommand { get; set; }

        public HeaderServicioEnum HeaderServicioEnum
        {
            get => headerServicioEnum;
            set => SetProperty(ref headerServicioEnum, value);
        }

        public ObservableRangeCollection<MiLoteriaGuardada> ListaMisLoterias
        {
            get => listaMisLoterias;
            set => SetProperty(ref listaMisLoterias, value);
        }
        public ICommand MasAccionesCommand => throw new NotImplementedException();

        public MiLoteriaGuardada MiLoteria { get => miLoteria; set => SetProperty(ref miLoteria, value); }

        public int Numero
        {
            get => numero;
            set
            {
                SetProperty(ref numero, value);
            }
        }

        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;
            set => SetProperty(ref _selectedViewModelIndex, value);
        }
        public int Serie
        {
            get => serie;
            set
            {
                SetProperty(ref serie, value);
            }
        }
        public string TipoJuego
        {
            get => tipoJuego;
            set
            {
                SetProperty(ref tipoJuego, value);
            }
        }
        void Agregar()
        {
            MiLoteria.Fecha = DateTime.Now;
            if (TipoJuego == "0")
            {
                if (MiLoteria.Numero.HasValue && MiLoteria.Serie.HasValue)
                {
                    MiLoteria.Tipo = "loterianacional";
                    ListaMisLoterias.Add(MiLoteria);
                    MiLoteria = new MiLoteriaGuardada();
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("¡Falta algo!", "Tenés que colocar todos los datos", "Cerrar");
                    return;
                }
            }
            else if (TipoJuego == "1")
            {
                if (MiLoteria.Numero.HasValue && MiLoteria.Serie.HasValue)
                {
                    MiLoteria.Tipo = "chances";
                    ListaMisLoterias.Add(MiLoteria);
                    MiLoteria = new MiLoteriaGuardada();
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("Falta algo!", "Tenés que colocar todos los datos", "Cerrar");
                    return;
                }
            }
            else if (TipoJuego == "2")
            {
                if (MiLoteria.Numero.HasValue && MiLoteria.Numero2.HasValue && MiLoteria.Numero3.HasValue && MiLoteria.Numero4.HasValue && MiLoteria.Numero5.HasValue)
                {
                    MiLoteria.Tipo = "lotto";
                    ListaMisLoterias.Add(MiLoteria);
                    MiLoteria = new MiLoteriaGuardada();
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("Falta algo!", "Tenés que colocar todos los datos", "Cerrar");
                    return;
                }
            }
            else if (TipoJuego == "3")
            {
                if (MiLoteria.Numero.HasValue && !string.IsNullOrEmpty(MiLoteria.Tanda))
                {
                    MiLoteria.Tipo = "nuevostiempos";
                    ListaMisLoterias.Add(MiLoteria);
                    MiLoteria = new MiLoteriaGuardada();
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("Falta algo!", "Tenés que colocar todos los datos", "Cerrar");
                    return;
                }
            }
            else if (TipoJuego == "4")
            {
                if (MiLoteria.Numero.HasValue && MiLoteria.Numero2.HasValue && MiLoteria.Numero3.HasValue && !string.IsNullOrEmpty(MiLoteria.Tanda))
                {
                    MiLoteria.Tipo = "Tres_Monazos";
                    ListaMisLoterias.Add(MiLoteria);
                    MiLoteria = new MiLoteriaGuardada();
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("Falta algo!", "Tenés que colocar todos los datos", "Cerrar");
                    return;
                }
            }

            _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Agregado en tu lista!");
            SyncData();
        }

        public void SyncData()
        {
            _ = Task.Run(async () =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await Locator.Instance.RestClient.PostAsync<RpSorteosMisNumeros>("/jps/guardarmisloterias", ListaMisLoterias);
                    foreach (var item in ListaMisLoterias)
                    {
                        item.Id = response.Lista.FirstOrDefault(p => p.Token == item.Token)?.Id ?? 0;
                        item.Ganador = response.Lista.FirstOrDefault(p => p.Token == item.Token)?.Ganador ?? false;
                        item.Notificado = response.Lista.FirstOrDefault(p => p.Token == item.Token)?.Notificado;
                        item.Sorteo = response.Lista.FirstOrDefault(p => p.Token == item.Token)?.Sorteo;
                    }
                    BlobCache.LocalMachine.InsertObject<List<MiLoteriaGuardada>>("MisLoterias", ListaMisLoterias.ToList());
                }
                else
                {
                    _ = Shell.Current.CurrentPage.DisplayToastAsync("¡No pudimos sincronizar los datos con el servidor! Verifica tu internet.");
                }
            });
        }

        async Task Cerrar()
        {
            await Shell.Current.GoToAsync("..");
        }

        async Task Eliminar(MiLoteriaGuardada item)
        {
            var respuesta = await Shell.Current.CurrentPage.DisplayActionSheet("¿Seguro? Lo vamos a eliminar para siempre", "No no!", "Sí, dale");
            if (respuesta == "Sí, dale")
            {
                ListaMisLoterias.Remove(item);
                BlobCache.LocalMachine.InsertObject<List<MiLoteriaGuardada>>("MisLoterias", ListaMisLoterias.ToList());
                _ = Shell.Current.CurrentPage.DisplayToastAsync("¡Ya lo volamos!");
                SyncData();
            }
        }
    }
}
