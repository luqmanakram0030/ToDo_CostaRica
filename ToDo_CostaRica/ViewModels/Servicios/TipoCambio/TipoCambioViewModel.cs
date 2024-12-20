using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Response;


namespace ToDo_CostaRica.ViewModels.Servicios.TipoCambio
{
    public class TipoCambioViewModel : ViewModelBase, IHeaderServicio
    {
        private List<RpTC> listaTC;

        public List<RpTC> ListaTC
        {
            get => listaTC;
            set => SetProperty(ref listaTC, value);
        }

        private string entidadActual;

        public string EntidadActual
        {
            get => entidadActual;
            set => SetProperty(ref entidadActual, value);
        }

        private string entidadActualCompra;

        public string EntidadActualCompra
        {
            get => entidadActualCompra;
            set => SetProperty(ref entidadActualCompra, value);
        }

        private string entidadActualVenta;

        public string EntidadActualVenta
        {
            get => entidadActualVenta;
            set => SetProperty(ref entidadActualVenta, value);
        }

        private string fechaActualizacion;

        public string FechaActualizacion
        {
            get => fechaActualizacion;
            set => SetProperty(ref fechaActualizacion, value);
        }

        public ICommand CerrarCommand { get; }

        public ICommand GoToFormularioCommand => throw new NotImplementedException();

        public ICommand GoToHistorialCommand => throw new NotImplementedException();

        public ICommand GoToInfoCommand => throw new NotImplementedException();

        HeaderServicioEnum headerServicioEnum;
        public HeaderServicioEnum HeaderServicioEnum
        {
            get => headerServicioEnum;
            set => SetProperty(ref headerServicioEnum, value);
        }

        public TipoCambioViewModel()
        {
            CerrarCommand = new AsyncRelayCommand(Cerrar);
            _ = InitPage();
        }

        private async Task InitPage()
        {
            await AiForms.Dialogs.Loading.Instance.StartAsync(async progress =>
            {
                try
                {
                    var response = await Locator.Instance.RestClient.PostAsync<RpObtenerTC>("/tipocambio/obtener");
                    Locator.GetInstance().TodoCRAds.CargarInterstitial();
                    LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
                    if (response.Status == "OK")
                    {
                        ListaTC = response.RpTC;
                        var entidadActual = ListaTC.FirstOrDefault(p => p.Entidad == "Banco Central de Costa Rica");
                        EntidadActual = entidadActual.Entidad;
                        EntidadActualCompra = $"₡{entidadActual.Compra}";
                        EntidadActualVenta = $"₡{entidadActual.Venta}";
                        FechaActualizacion = $"Fecha: {entidadActual.Fecha:dd/MM/yyyy}";
                    }
                    else
                    {
                        _ = Shell.Current.CurrentPage.DisplayAlert("Un problema ha sucedido al obtener la información", "OK", null);
                        await Shell.Current.GoToAsync("..");
                    }
                }
                catch (Exception)
                {
                    _ = Shell.Current.CurrentPage.DisplayAlert("Un problema ha sucedido al obtener la información", "OK", null);
                    await Shell.Current.GoToAsync("..");
                }
            });
        }

        //async Task Cerrar()
        //{
        //    //await Shell.Current.GoToAsync("..");
        //    await App.Current.MainPage.Navigation.PopAsync(true);
        //}

        private async Task Cerrar()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            await Shell.Current.GoToAsync("..");
        }

        public virtual bool OnBackButtonPressed()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            return true;
        }
    }
}