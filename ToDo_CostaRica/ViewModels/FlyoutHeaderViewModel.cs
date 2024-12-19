using ToDo_CostaRica.Infrastructure;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels
{
    public class FlyoutHeaderViewModel : ViewModelBase
    {
        private string nombre;
        private string email;
        private string foto;

        public string Nombre
        {
            get => nombre;
            set => SetProperty(ref nombre, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Foto
        {
            get => foto;
            set => SetProperty(ref foto, value);
        }

        public FlyoutHeaderViewModel()
        {
            MessagingCenter.Subscribe<object>(this, "SetLoginUser", (sender) =>
            {
                Nombre = Locator.Instance.User.Email == "Anonimo" ? "Invitado(a)" : Locator.Instance.User.Email;
                Email = Locator.Instance.User.Email == "Anonimo" ? "Sincroniza tus datos registrando una cuenta" : "Estas autenticado";
            });
        }
    }
}