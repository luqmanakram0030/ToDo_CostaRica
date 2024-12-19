using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ToDo_CostaRica.Interfaces
{
    public interface IConsultaServicio
    {
        ICommand MasAccionesCommand { get; }
    }
}
