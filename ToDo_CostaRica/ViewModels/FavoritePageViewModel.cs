using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ToDo_CostaRica.Models;

namespace ToDo_CostaRica.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
    {
        private ObservableCollection<FavouriteItem> favourites;

        public ObservableCollection<FavouriteItem> Favourites
        {
            get { return favourites; }
            set { this.favourites = value; }
        }
        public FavoritePageViewModel()
        {
            Favourites = new ObservableCollection<FavouriteItem>()
            {
                new FavouriteItem()
                {
                    Icon = "carsearch.png",
                    Title = "Car Search"
                },
                new FavouriteItem()
                {
                    Icon = "govinsurance.png",
                    Title = "Insurance"
                },
                new FavouriteItem()
                {
                    Icon = "govtaxes.png",
                    Title = "Taxes"
                }
            };
        }
    }
}
