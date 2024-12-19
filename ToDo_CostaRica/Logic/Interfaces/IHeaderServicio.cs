using System.Windows.Input;
using ToDo_CostaRica.Models;

namespace ToDo_CostaRica.Interfaces
{
    public interface IHeaderServicio
    {
        ICommand GoToFormularioCommand { get; }
        ICommand GoToHistorialCommand { get; }
        ICommand GoToInfoCommand { get; }
        ICommand CerrarCommand { get; }
        HeaderServicioEnum HeaderServicioEnum { get; set; }
    }
}
