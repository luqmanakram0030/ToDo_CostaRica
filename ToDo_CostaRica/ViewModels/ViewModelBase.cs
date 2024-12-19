
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Services;


namespace ToDo_CostaRica.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        bool literalAdVisible;
        
        public bool LiteralAdVisible
        {
            get => literalAdVisible;
            set => SetProperty(ref literalAdVisible, value);
        }

        public ICommand EjecutarCommand { get; set; }
    }
}
