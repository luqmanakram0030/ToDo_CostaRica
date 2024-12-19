using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Models;
using ToDo_CostaRica.Views.Settings;
using ToDoCR.SharedDomain.Models;
using ToDoCR.SharedDomain.Response;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace ToDo_CostaRica.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private ObservableRangeCollection<Opinion> opinionLog;
        public ObservableRangeCollection<Opinion> OpinionLog
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
            OpinionLog = new ObservableRangeCollection<Opinion>();
            ComentarCommand = new AsyncCommand(Comentar);
            RefreshCommand = new AsyncCommand(CargarData);
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
                    var response = await Locator.Instance.RestClient.PostAsync<RpOpinion>("/user/opinions");
                    opinionLog.ReplaceRange(response.Opinions);
                }
                catch (Exception)
                {
                    await Shell.Current.CurrentPage.DisplaySnackBarAsync("No pudimos obtener tus comentarios. Verifica tu internet", "OK", null);
                }
                IsBusy = false;
                CanLoadMore = false;
            }
        }

        async Task Comentar()
        {
            await PopupNavigation.Instance.PushAsync(new AgregarOpinionPopup());
        }
    }
}
