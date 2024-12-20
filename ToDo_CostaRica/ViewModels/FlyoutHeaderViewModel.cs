using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ToDo_CostaRica.Infrastructure;

namespace ToDo_CostaRica.ViewModels
{
    public partial class FlyoutHeaderViewModel : ObservableObject
    {
        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string foto;

        public FlyoutHeaderViewModel()
        {
            // Subscribe to a message to set login user details
            WeakReferenceMessenger.Default.Register<SetLoginUserMessage>(this, (r, message) =>
            {
                Nombre = Locator.Instance.User.Email == "Anonimo" ? "Invitado(a)" : Locator.Instance.User.Email;
                Email = Locator.Instance.User.Email == "Anonimo" 
                    ? "Sincroniza tus datos registrando una cuenta" 
                    : "Estas autenticado";
            });
        }
    }

    // Define a custom message for user login updates
    public class SetLoginUserMessage
    {
    }
}