using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Interfaces;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Models;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels.Servicios.Calcs
{
    public class SalarialViewModel : ViewModelBase, IConsultaServicio, IHeaderServicio
    {
        CalcSalarial calcSalarial;
        public ICommand MasAccionesCommand => throw new NotImplementedException();

        public CalcSalarial CalcSalarial
        {
            get => calcSalarial;
            set => SetProperty(ref calcSalarial, value);
        }

        decimal? salario;
        public decimal? Salario
        {
            get => salario;
            set
            {
                SetProperty(ref salario, value);
                if (value > 0)
                {
                    CalcSalarial = new CalcSalarial(value.Value);
                }
            }
        }

        public ICommand GoToFormularioCommand => throw new NotImplementedException();

        public ICommand GoToHistorialCommand => throw new NotImplementedException();

        public ICommand GoToInfoCommand => throw new NotImplementedException();

        public ICommand CerrarCommand { get; }
        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SalarialViewModel()
        {
            Title = "Calculadora Salarial";
            CalcSalarial = new CalcSalarial();
            CerrarCommand = new AsyncCommand(Cerrar);
        }

        async Task Cerrar()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
