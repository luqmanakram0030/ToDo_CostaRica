using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Services;


namespace ToDo_CostaRica.ViewModels
{
    [QueryProperty("Id", "id")]
    [QueryProperty(nameof(Titletext), nameof(titletext))]
    public class SubCategoryViewModel : ViewModelBase
    {
        int id;
        public int Id
        {
            set
            {
                id = value;
                CargarSubCategorias(value);
            }
            get => id;

        }

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

        private ObservableCollection<SubCategory> categoryItems;

        public ObservableCollection<SubCategory> CategoryItems
        {
            get  => categoryItems;
            set => SetProperty(ref categoryItems, value);
        }

        public SubCategoryViewModel()
        {
            //CategoryItems = new ObservableCollection<SubCategory>()
            //{
            //    new SubCategory()
            //    {
            //        Id = 1,
            //        Title = "Consulta civil",
            //        Img = "govpersons.png"
            //    },
            //    //new SubCategory()
            //    //{
            //    //    Id = 1,
            //    //    Title = "Situación tributaria",
            //    //    Img = "socialsecurity.png"
            //    //},
            //    new SubCategory()
            //    {
            //        Id = 2,
            //        Title = "Situación tributaria",
            //        Img = "govtaxes.png"
            //    },
            //    new SubCategory()
            //    {
            //        Id = 3,
            //        Title = "Marchamo",
            //        Img = "carsearch.png",
            //        IsFavourite = true
            //    },
            //    //new SubCategory()
            //    //{
            //    //    Id = 4,
            //    //    Title = "Gov. Insurance",
            //    //    Img = "govinsurance.png"
            //    //},
            //    //new SubCategory()
            //    //{
            //    //    Id = 5,
            //    //    Title = "Gov. House",
            //    //    Img = "govhouse.png"
            //    //}
            //};
        }

        private void CargarSubCategorias(int value)
        {
            CategoryItems = new ObservableCollection<SubCategory>(AvailableServices.AppServicesList.First(p => p.Id == value));
        }
    }
}
