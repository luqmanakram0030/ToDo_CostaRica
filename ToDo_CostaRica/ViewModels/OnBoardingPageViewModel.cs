using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ToDo_CostaRica.Models;

namespace ToDo_CostaRica.ViewModels
{
    public class OnBoardingPageViewModel : ViewModelBase
    {
        private ObservableCollection<OnboardingItem> onboardingItems;

        public ObservableCollection<OnboardingItem> OnboardingItems
        {
            get { return onboardingItems; }
            set { this.onboardingItems = value; }
        }
        public OnBoardingPageViewModel()
        {
            OnboardingItems = new ObservableCollection<OnboardingItem>
            {
                new OnboardingItem()
                {
                    Id = 0,
                    Img = "onboarding_1.png",
                    Title = "Todo Consultas de Costa Rica",
                    Description = "Busca en un solo lugar la información que necesitas."
                },
                new OnboardingItem()
                {
                    Id = 1,
                    Img = "onboarding_2.png",
                    Title = "Herramientas de todo tipo",
                    Description = "Desde consulta de personas hasta calculadoras salariales"
                },
                new OnboardingItem()
                {
                    Id = 2,
                    Img = "onboarding_2.png",
                    Title = "Notificaciones de la lotería",
                    Description = "Sé notificado cuando los resultados estén disponibles"
                },
                new OnboardingItem()
                {
                    Id = 3,
                    Img = "onboarding_3.png",
                    Title = "Puedes crear una cuenta",
                    Description = "Si deseas sincronizar los datos, puedes crearte una cuenta",
                }, new OnboardingItem()
                {
                    Id = 4,
                    Img = "onboarding_3.png",
                    Title = "Explora la app",
                    Description = "Con el tiempo, agregaremos más consultas, comienza ya",
                    IsLastItem = true
                }
            };
        }
    }
}
