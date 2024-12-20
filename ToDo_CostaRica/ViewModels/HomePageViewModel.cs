using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Services;

namespace ToDo_CostaRica.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private ObservableCollection<AppService> popularServices;
        private ObservableCollection<Banner> banners;
        private ObservableCollection<AppService> recentlyVisited;

        public ObservableCollection<AppService> PopularServices
        {
            get { return popularServices; }
            set { SetProperty(ref this.popularServices, value); }
        }

        public ObservableCollection<Banner> Banners
        {
            get { return banners; }
            set { SetProperty(ref this.banners, value); }
        }

        public ObservableCollection<AppService> RecentlyVisited
        {
            get { return recentlyVisited; }
            set { SetProperty(ref this.recentlyVisited, value); }
        }

        public HomePageViewModel()
        {
            InitLoadingData();
            //OnLoadCommandExecute();
        }

        public void OnAppearing()
        {
            _ = OnLoadCommandExecute();
        }

        void InitLoadingData()
        {
            PopularServices = new ObservableCollection<AppService>()
            {
                new AppService()
                {
                    Img = "----",
                    Title = "----",
                    IsBusy = true
                },
                new AppService()
                {
                    Img = "----",
                    Title = "----",
                    IsBusy = true
                },
                new AppService()
                {
                    Img = "----",
                    Title = "----",
                    IsBusy = true
                }
            };
            Banners = new ObservableCollection<Banner>()
            {
                new Banner()
                {
                    Img = "banner1.png",
                    IsBusy = true
                },
                new Banner()
                {
                    Img = "banner1.png",
                    IsBusy = true
                },
                new Banner()
                {
                    Img = "banner1.png",
                    IsBusy = true
                }
            };
            RecentlyVisited = new ObservableCollection<AppService>()
            {
                new AppService()
                {
                    Img = "serviceimg4.png",
                    IsBusy = true
                },
                new AppService()
                {
                    Img = "serviceimg5.png",
                    IsBusy = true
                },
                new AppService()
                {
                    Img = "serviceimg6.png",
                    IsBusy = true
                }
            };
          //  IsBusy = true;
        }

        protected async Task OnLoadCommandExecute()
        {
            await Task.Delay(5000);

            PopularServices = new ObservableCollection<AppService>()
            {
                new AppService()
                {
                    Img = "serviceimg1.png",
                    Title = "House\nCleaning"
                },
                new AppService()
                {
                    Img = "serviceimg2.png",
                    Title = "House\nPainting"
                },
                new AppService()
                {
                    Img = "serviceimg3.png",
                    Title = "Win\nLottery"
                }
            };

            Banners = new ObservableCollection<Banner>()
            {
                new Banner()
                {
                    Img = "banner1.png"
                },
                new Banner()
                {
                    Img = "banner2.png"
                },
                new Banner()
                {
                    Img = "banner1.png"
                }
            };

            RecentlyVisited = new ObservableCollection<AppService>()
            {
                new AppService()
                {
                    Img = "serviceimg4.png",
                    Title = "Bill Payment\nServices"
                },
                new AppService()
                {
                    Img = "serviceimg5.png",
                    Title = "Mobile\nServices"
                },
                new AppService()
                {
                    Img = "serviceimg6.png",
                    Title = "Medicle\nServices"
                }
            };

           // IsBusy = false;
        }
    }
}
