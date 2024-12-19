
using System.Collections.ObjectModel;

using ToDoCR.SharedDomain.Services;


namespace ToDo_CostaRica.ViewModels
{
    [QueryProperty(nameof(Titletext), nameof(titletext))]
    public class SeeAllPageViewModel : ViewModelBase
    {
        string titletext = "";
        public string Titletext
        {
            get => titletext;
            set
            {
                titletext = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AppService> popularServices;
        private ObservableCollection<AppService> recentlyVisited;

        public ObservableCollection<AppService> PopularServices
        {
            get { return popularServices; }
            set { this.popularServices = value; }
        }

        public ObservableCollection<AppService> RecentlyVisited
        {
            get { return recentlyVisited; }
            set { this.recentlyVisited = value; }
        }

        public SeeAllPageViewModel()
        {
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
        }
    }
}
