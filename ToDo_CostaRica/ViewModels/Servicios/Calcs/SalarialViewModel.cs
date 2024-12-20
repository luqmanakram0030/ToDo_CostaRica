using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Models;
using Mopups.Services;

namespace ToDo_CostaRica.ViewModels.Servicios.Calcs
{
    public partial class SalarialViewModel : ObservableObject, IConsultaServicio, IHeaderServicio
    {
        private CalcSalarial calcSalarial;

        [ObservableProperty]
        private decimal? salario;

        public CalcSalarial CalcSalarial
        {
            get => calcSalarial;
            set => SetProperty(ref calcSalarial, value);
        }

        public ICommand CerrarCommand { get; }

        public ICommand MasAccionesCommand => throw new NotImplementedException();
        public ICommand GoToFormularioCommand => throw new NotImplementedException();
        public ICommand GoToHistorialCommand => throw new NotImplementedException();
        public ICommand GoToInfoCommand => throw new NotImplementedException();
        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SalarialViewModel()
        {
           // Title = "Calculadora Salarial";
            CalcSalarial = new CalcSalarial();
            CerrarCommand = new AsyncRelayCommand(Cerrar);

            Salario = null;
        }

        partial void OnSalarioChanged(decimal? value)
        {
            if (value > 0)
            {
                CalcSalarial = new CalcSalarial(value.Value);
                OnPropertyChanged(nameof(CalcSalarial));
            }
        }

        private async Task Cerrar()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}