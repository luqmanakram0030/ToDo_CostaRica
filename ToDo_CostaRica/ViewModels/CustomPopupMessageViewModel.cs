using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ToDo_CostaRica.ViewModels
{
    public partial class CustomPopupMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string image;

        [ObservableProperty]
        private string boton;

        public CustomPopupMessageViewModel(string title, string message, string image, string boton)
        {
            Title = title;
            Message = message;
            Image = image;
            Boton = boton;
        }

        public string Title { get; set; }

        [RelayCommand]
        private async Task Close()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}