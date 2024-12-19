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
    public class PatronalViewModel : ViewModelBase, IConsultaServicio, IHeaderServicio
    {
        CalcPatronal calcPatronal;
        public ICommand MasAccionesCommand => throw new NotImplementedException();

        public CalcPatronal CalcPatronal
        {
            get => calcPatronal;
            set => SetProperty(ref calcPatronal, value);
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
                    if (CalcPatronal == null)
                    {
                        CalcPatronal = new CalcPatronal();
                    }
                    CalcPatronal.Calcular(value.Value);
                    OnPropertyChanged("CalcPatronal");
                }
            }
        }

        public ICommand GoToFormularioCommand => throw new NotImplementedException();

        public ICommand GoToHistorialCommand => throw new NotImplementedException();

        public ICommand GoToInfoCommand => throw new NotImplementedException();

        public ICommand CerrarCommand { get; }
        public HeaderServicioEnum HeaderServicioEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public PatronalViewModel()
        {
            Title = "Calculadora Patronal";
            CalcPatronal = new CalcPatronal();
            CerrarCommand = new AsyncCommand(Cerrar);
        }

        async Task Cerrar()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
