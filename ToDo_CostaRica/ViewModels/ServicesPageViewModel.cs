using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Services;

namespace ToDo_CostaRica.ViewModels
{
    public class ServicesPageViewModel : ViewModelBase
    {

        private ObservableCollection<AppService> services;

        public ObservableCollection<AppService> Services
        {
            get { return services; }
            set { this.services = value; }
        }
        public ServicesPageViewModel()
        {
            Services = new ObservableCollection<AppService>(AvailableServices.AppServicesList);

            //Services = new ObservableCollection<Service>
            //{
            //    new Service()
            //    {
            //        Id = 1,
            //        Img = "icon1.png",
            //        Title = "Gobierno"
            //    },
            //    //new Service()
            //    //{
            //    //    Id = 2,
            //    //    Img = "icon2.png",
            //    //    Title = "Serv. Públicos"
            //    //},
            //    new Service()
            //    {
            //        Id = 3,
            //        Img = "icon3.png",
            //        Title = "Lotería"
            //    },
            //    new Service()
            //    {
            //        Id = 4,
            //        Img = "icon4.png",
            //        Title = "Calculadoras"
            //    },
            //    new Service()
            //    {
            //        Id = 5,
            //        Img = "icon15.png",
            //        Title = "Combustibles"
            //    },
            //    //new Service()
            //    //{
            //    //    Id = 4,
            //    //    Img = "icon5.png",
            //    //    Title = "Educación"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 5,
            //    //    Img = "icon6.png",
            //    //    Title = "Médicos"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 6,
            //    //    Img = "icon7.png",
            //    //    Title = "Tiquetes"
            //    //},
            //    new Service()
            //    {
            //        Id = 7,
            //        Img = "icon8.png",
            //        Title = "Tipo de cambio"
            //    },
            //    //new Service()
            //    //{
            //    //    Id = 8,
            //    //    Img = "icon9.png",
            //    //    Title = "Agricultura"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 9,
            //    //    Img = "icon10.png",
            //    //    Title = "Turismo"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 10,
            //    //    Img = "icon11.png",
            //    //    Title = "Religión"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 11,
            //    //    Img = "icon12.png",
            //    //    Title = "Comidas"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 12,
            //    //    Img = "icon13.png",
            //    //    Title = "Clima"
            //    //},
            //    //new Service()
            //    //{
            //    //    Id = 13,
            //    //    Img = "icon14.png",
            //    //    Title = "Petroleo"
            //    //},
                
            //};
        }
    }
}
