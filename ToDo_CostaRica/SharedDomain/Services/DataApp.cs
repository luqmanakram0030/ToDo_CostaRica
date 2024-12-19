using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCR.SharedDomain.Services
{
    public static class AvailableServices
    {
        public static List<AppService> AppServicesList { get; }
        static AvailableServices()
        {
            AppServicesList = new List<AppService>() {

                new AppService(
                1,
                "icon1.png",
                "Gobierno",
                null,
                new List<SubCategory>()
                {
                    new SubCategory()
                    {
                        Id = 11,
                        Title = "Consulta civil",
                        Img = "govpersons.png",
                        ViewName = "PersonaCivilPage",
                    },
                    //new SubCategory()
                    //{
                    //    Id = 12,
                    //    Title = "Situación tributaria",
                    //    Img = "govtaxes.png",
                    //    ViewName = "PersonaCivilPage",
                    //},
                    new SubCategory()
                    {
                        Id = 13,
                        Title = "Morosidad patronal",
                        Img = "govtaxes.png",
                        ViewName = "MorosidadPatronal",
                    },
                    //new SubCategory()
                    //{
                    //    Id = 14,
                    //    Title = "Marchamo",
                    //    Img = "carsearch.png",
                    //    ViewName = "ServiceSearchPage",
                    //},
                }),

                new AppService(
                3,
                "icon3.png",
                "Lotería",
                null,
                new List<SubCategory>()
                {
                    new SubCategory()
                    {
                        Id = 336,
                        Title = "Mis números",
                        Img = "notification.png",
                        ViewName = "LoteriaMisNumeros",
                    },
                    new SubCategory()
                    {
                        Id = 31,
                        Title = "Lotería Nacional",
                        Img = "loteria_logo_loteria.png",
                        ViewName = "LoteriaDashboard",
                        Parameters = "tipo=loterianacional&img=loteria_logo_loteria.png&Title=Lotería Nacional"
                    },new SubCategory()
                    {
                        Id = 32,
                        Title = "Chances",
                        Img = "loteria_logo_chances.png",
                        ViewName = "LoteriaDashboard",
                        Parameters = "tipo=chances&img=loteria_logo_chances.png&Title=Chances"
                    },new SubCategory()
                    {
                        Id = 33,
                        Title = "Lotto",
                        Img = "loteria_logo_lotto.png",
                        ViewName = "LoteriaDashboard",
                        Parameters = "tipo=lotto&img=loteria_logo_lotto.png&Title=Lotto"
                    },new SubCategory()
                    {
                        Id = 34,
                        Title = "Nuevos Tiempos",
                        Img = "loteria_Logo_Nuevos_Tiempos_Reventados.png&Title=Nuevos Tiempos",
                        ViewName = "LoteriaDashboard",
                        Parameters = "tipo=nuevostiempos&img=loteria_Logo_Nuevos_Tiempos_Reventados.png"
                    },new SubCategory()
                    {
                        Id = 35,
                        Title = "3 Monazos",
                        Img = "loteria_logo_tresMonazos.png",
                        ViewName = "LoteriaDashboard",
                        Parameters = "tipo=Tres_Monazos&img=loteria_logo_tresMonazos.png&Title=Tres Monazos"
                    },
                }),

                new AppService(
                4,
                "icon4.png",
                "Calculadoras",
                null,
                new List<SubCategory>()
                {
                    //new SubCategory()
                    //{
                    //    Id = 1,
                    //    Title = "Prestamos",
                    //    Img = "govpersons.png"
                    //},
                    new SubCategory()
                    {
                        Id = 2,
                        Title = "Salarial",
                        Img = "govtaxes.png",
                        ViewName = "Salarial",
                    },
                    new SubCategory()
                    {
                        Id = 2,
                        Title = "Patronal",
                        Img = "govtaxes.png",
                        ViewName = "Patronal",
                    },
                }),

                new AppService(
                5,
                "icon5.png",
                "Combustibles",
                "CombustiblesDashboard",
                new List<SubCategory>()),

                new AppService(
                8,
                "icon8.png",
                "Tipo de cambio",
                "TipoCambioDashboard",
                new List<SubCategory>())

            };
        }
    }

    public class AppService : List<SubCategory>
    {
        public AppService()
        {

        }

        public AppService(int id, string img, string title, string viewName, List<SubCategory> subCategories) : base(subCategories)
        {
            Id = id;
            Img = img;
            Title = title;
            ViewName = viewName;
        }

        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string ViewName { get; set; }
        public bool IsBusy { get; set; }
    }

    public class SubCategory
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public bool IsFavourite { get; set; }
        public string ViewName { get; set; }
        public string Parameters { get; set; }
    }
}
