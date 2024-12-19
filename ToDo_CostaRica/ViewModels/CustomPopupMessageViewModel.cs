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

namespace ToDo_CostaRica.ViewModels
{
    public class CustomPopupMessageViewModel : ViewModelBase
    {
        private string message;
        private string image;
        private string boton;

        public CustomPopupMessageViewModel(string title, string message, string image, string boton)
        {
            Title = title;
            this.message = message;
            this.image = image;
            this.boton = boton;
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
        public string Boton
        {
            get => boton;
            set => SetProperty(ref boton, value);
        }

        async Task Cerrar()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
