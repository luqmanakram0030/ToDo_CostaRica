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
    public partial class PatronalViewModel : ObservableObject, IConsultaServicio, IHeaderServicio
    {
        private CalcPatronal calcPatronal;

        [ObservableProperty]
        private decimal? salario;

        public CalcPatronal CalcPatronal
        {
            get => calcPatronal;
            set => SetProperty(ref calcPatronal, value);
        }

        public ICommand CerrarCommand { get; }

        public ICommand MasAccionesCommand => throw new NotImplementedException();
        public ICommand GoToFormularioCommand => throw new NotImplementedException();
        public ICommand GoToHistorialCommand => throw new NotImplementedException();
        public ICommand GoToInfoCommand => throw new NotImplementedException();
        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public PatronalViewModel()
        {
           // Title = "Calculadora Patronal";
            CalcPatronal = new CalcPatronal();
            CerrarCommand = new AsyncRelayCommand(Cerrar);

            Salario = null;
        }

        partial void OnSalarioChanged(decimal? value)
        {
            if (value > 0)
            {
                if (CalcPatronal == null)
                {
                    CalcPatronal = new CalcPatronal();
                }
                CalcPatronal.Calcular(value.Value);
                OnPropertyChanged(nameof(CalcPatronal));
            }
        }

        private async Task Cerrar()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}