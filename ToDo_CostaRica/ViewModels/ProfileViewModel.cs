
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Views.Settings;
using ToDoCR.SharedDomain.Models;
using ToDoCR.SharedDomain.Response;


namespace ToDo_CostaRica.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private ObservableCollection<Opinion> opinionLog;
        public ObservableCollection<Opinion> OpinionLog
        {
            get => opinionLog;
            set => SetProperty(ref opinionLog, value);
        }

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set => SetProperty(ref nombre, value);
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string cel;

        public string Cel
        {
            get => cel;
            set => SetProperty(ref cel, value);
        }

        private bool registrered;

        public bool Registrered
        {
            get => registrered;
            set => SetProperty(ref registrered, value);
        }
        public ICommand ComentarCommand { get; }
        public ICommand RefreshCommand { get; }

        public ProfileViewModel()
        {
            Registrered = Locator.Instance.User.LoggedIn;
            Nombre = Locator.Instance.User.Nombre;
            Email = Locator.Instance.User.Email;
            Cel = Locator.Instance.User.Cel;
            OpinionLog = new ObservableCollection<Opinion>();
            ComentarCommand = new AsyncRelayCommand(Comentar);
            RefreshCommand = new AsyncRelayCommand(CargarData);
            _ = CargarData();

            //OpinionLog = new ObservableCollection<Opinion>()
            //{
            //    new Opinion()
            //    {
            //        Date = "25 Jul, 2021",
            //        OpinionDescription = "Me gustaría poder contar con la consulta de la lotería.",
            //        HasAdminReply = true,
            //        AdminReplyDate = "26 Jul, 2021",
            //        AdminReply = "Hola! Gracias por escribirnos. Para nosotros sería genial poder aportar esa nueva funcionalidad. Mantente atento."
            //    }
            //};
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if(SetProperty(ref _isBusy, value,"IsBusy"))
                {
                    IsNotBusy= !_isBusy;
                }
                
            }
           
        }
        private bool _isNotBusy;
        public bool IsNotBusy
        {
            get
            {
                return _isNotBusy;
            }
            set
            {
                if(SetProperty(ref _isNotBusy, value,"IsBusy"))
                {
                    IsBusy= !_isNotBusy;
                }
                
            }
           
        }
        public async Task CargarData()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    Registrered = Locator.Instance.User.LoggedIn;
                    Nombre = Locator.Instance.User.Nombre;
                    Email = Locator.Instance.User.Email;
                    Cel = Locator.Instance.User.Cel;

                    // Fetch opinions from the server
                    var response = await Locator.Instance.RestClient.PostAsync<RpOpinion>("/user/opinions");

                    // Clear existing opinions and add new ones
                    OpinionLog.Clear();
                    foreach (var opinion in response.Opinions)
                    {
                        OpinionLog.Add(opinion);
                    }
                }
                catch (Exception)
                {
                    await Shell.Current.CurrentPage.DisplayAlert("No pudimos obtener tus comentarios. Verifica tu internet", "OK", null);
                }
                IsBusy = false;
               // CanLoadMore = false;
            }
        }

        async Task Comentar()
        {
            await MopupService.Instance.PushAsync(new AgregarOpinionPopup());
        }
    }
}
