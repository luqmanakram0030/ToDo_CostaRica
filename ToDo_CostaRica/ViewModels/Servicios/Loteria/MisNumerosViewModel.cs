using Akavache;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Views;
using ToDo_CostaRica.Views.Servicios.Loteria;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Models;
using ToDoCR.SharedDomain.Models.Loteria;
using ToDoCR.SharedDomain.Response;

using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.ViewModels.Servicios.Loteria
{ 

    public class MisNumerosViewModel  : ObservableObject, IHeaderServicio, IConsultaServicio
    {
        private int _selectedViewModelIndex = 0;
        int formulario;
        int fracciones;
        HeaderServicioEnum headerServicioEnum;
        ObservableCollection<MiLoteriaGuardada> listaMisLoterias;
        MiLoteriaGuardada miLoteria;
        int numero;
        int serie;
        string tipoJuego;
        string Title;
        public MisNumerosViewModel()
        {
            Title = "Mis números";
            Fracciones = 1;
            Formulario = 0;
            //HeaderServicioEnum = HeaderServicioEnum.Buscando;
            CerrarCommand = new AsyncRelayCommand(Cerrar);
            AgregarCommand = new Command(Agregar);
            EliminarCommand = new AsyncRelayCommand<MiLoteriaGuardada>(Eliminar);
            MiLoteria = new MiLoteriaGuardada();
            listaMisLoterias = new ObservableCollection<MiLoteriaGuardada>();
            BlobCache.LocalMachine.GetObject<List<MiLoteriaGuardada>>("MisLoterias")
                .Subscribe(x =>
                {
                    // Clear the existing collection and add the new data
                    ListaMisLoterias.Clear();
                    foreach (var item in x.OrderBy(p => p.Tipo))
                    {
                        ListaMisLoterias.Add(item);
                    }

                    // Check network access and perform sync if necessary
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        if (ListaMisLoterias.Any(p => p.Id == 0)) // Not sent to server
                        {
                            SyncData();
                        }

                        if (ListaMisLoterias.Any(p => !p.Notificado.HasValue)) // There are draws without notification
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

        public ObservableCollection<MiLoteriaGuardada> ListaMisLoterias
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

            _ = Toast.Make("¡Agregado en tu lista!", ToastDuration.Short).Show();
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
                    _ = Toast.Make("¡No pudimos sincronizar los datos con el servidor! Verifica tu internet.", ToastDuration.Long).Show();  }
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
                _ = Toast.Make("¡Ya lo volamos!", ToastDuration.Short).Show();
                SyncData();
            }
        }
    }
}
