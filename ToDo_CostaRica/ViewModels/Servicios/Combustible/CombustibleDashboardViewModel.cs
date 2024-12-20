using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Models.Recope;
using CommunityToolkit.Maui.Alerts;
using Mopups.Services;

namespace ToDo_CostaRica.ViewModels.Servicios.Combustible
{
    public partial class CombustibleDashboardViewModel : ObservableObject, IHeaderServicio
    {


        [ObservableProperty]
        public List<RecopePrecioConsumidor> lista;
        

        public ICommand CerrarCommand { get; }

        public ICommand GoToFormularioCommand => throw new NotImplementedException();
        public ICommand GoToHistorialCommand => throw new NotImplementedException();
        public ICommand GoToInfoCommand => throw new NotImplementedException();
        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CombustibleDashboardViewModel()
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
                    var response = await Locator.Instance.RestClient.PostAsync<List<RecopePrecioConsumidor>>("/recope/precioconsumidor");
                    Locator.GetInstance().TodoCRAds.CargarInterstitial();
                    LiteralAdVisible = Locator.GetInstance().TodoCRAds.VaAMostrarAnuncio();
                    if (response?.Any() == true)
                    {
                        Lista = response;
                    }
                    else
                    {
                        var toast = Toast.Make("Un problema ha sucedido al obtener la información", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                        await toast.Show();
                        await Shell.Current.GoToAsync("..");
                    }
                }
                catch (Exception)
                {
                    var toast = Toast.Make("Un problema ha sucedido al obtener la información", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                    await toast.Show();
                    await Shell.Current.GoToAsync("..");
                }
            });
        }

        private async Task Cerrar()
        {
            Locator.GetInstance().TodoCRAds.MostrarInterstitial();
            await MopupService.Instance.PopAsync();
        }
    }
}