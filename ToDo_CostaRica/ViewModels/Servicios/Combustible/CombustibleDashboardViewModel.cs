using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Models.Recope;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Servicios.Combustible
{
    public class CombustibleDashboardViewModel : ViewModelBase, IHeaderServicio
    {
        private List<RecopePrecioConsumidor> lista;

        public List<RecopePrecioConsumidor> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }

        public ICommand GoToFormularioCommand => throw new NotImplementedException();

        public ICommand GoToHistorialCommand => throw new NotImplementedException();

        public ICommand GoToInfoCommand => throw new NotImplementedException();

        public ICommand CerrarCommand { get; }

        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CombustibleDashboardViewModel()
        {
            CerrarCommand = new AsyncCommand(Cerrar);
            _ = InitPage();
        }

        private async Task InitPage()
        {
            await AiForms.Dialogs.Loading.Instance.StartAsync(async progress =>
            {
                try
                {
                    var response = await Locator.Instance.RestClient.PostAsync<List<RecopePrecioConsumidor>>("/recope/precioconsumidor");
                    Locator.GetInstance().TodoCRAds.CargarInterstitial();
                    LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
                    if (response?.Any() == true)
                    {
                        Lista = response;
                    }
                    else
                    {
                        _ = Shell.Current.CurrentPage.DisplaySnackBarAsync("Un problema ha sucedido al obtener la información", "OK", null);
                        await Shell.Current.GoToAsync("..");
                    }
                }
                catch (Exception)
                {
                    _ = Shell.Current.CurrentPage.DisplaySnackBarAsync("Un problema ha sucedido al obtener la información", "OK", null);
                    await Shell.Current.GoToAsync("..");
                }
            });
        }

        private async Task Cerrar()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            await Shell.Current.GoToAsync("..");
        }
    }
}
