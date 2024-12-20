
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using Plugin.Maui.Audio;
using ToDo_CostaRica.Infrastructure;
using ToDo_CostaRica.Views;
using ToDoCR.SharedDomain;
using ToDoCR.SharedDomain.Databases.Local;
using ToDoCR.SharedDomain.Response;


namespace ToDo_CostaRica.ViewModels.Settings
{
    public class AgregarOpinionViewModel : ViewModelBase
    {
        private readonly IAudioManager _audioManager;
        string comentario;
        public ICommand AgregarCommand { get; }
        public ICommand CerrarCommand { get; }

        public string Comentario
        {
            get => comentario;
            set => SetProperty(ref comentario, value);
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
        public AgregarOpinionViewModel(IAudioManager _audioManager)
        {
            this._audioManager = _audioManager;
            AgregarCommand = new AsyncRelayCommand(Agregar);
            CerrarCommand = new AsyncRelayCommand(CerrarClicked);
           
        }
        
        async Task CerrarClicked()
        {
            await MopupService.Instance.PopAsync();
        }
        private async Task PlayErrorSound()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.wav"));
            player.Play();
        }
        async Task Agregar()
        {
            if (string.IsNullOrEmpty(comentario))
            {
                await PlayErrorSound();
                _ = Toast.Make("¡Escribe tu comentario!", ToastDuration.Short).Show();
                return;
            }
            IsBusy = true;
            try
            {
               
                _ = Toast.Make("¡Guardando, espera!", ToastDuration.Short).Show();
                RpComment response = await Locator.Instance.RestClient.PostAsync<RpComment>("/user/add-comment", new
                {
                    Comment = comentario,
                });
                if (response.Status == "OK")
                {
                    await Shell.Current.CurrentPage.DisplayAlert("¡Recibido!", "Ya lo tenemos guardado, te responderemos pronto.", "Continuar");
                    await MopupService.Instance.PopAsync();
                }
                else
                {
                    await PlayErrorSound();
                    
                    _ = Toast.Make("Intenta de nuevo, no lo logramos.", ToastDuration.Short).Show();
                }
            }
            catch (Exception)
            {
                //await Shell.Current.CurrentPage.DisplayToastAsync("Lo sentimos, no pudimos contactar el servicio.");
                await PlayErrorSound();
                _ = Locator.MostrarPopupGenerico("Algo salio mal", "No pudimos contactar el servicio, lamentamos mucho lo que sucedio. Intenta más tarde.", "sadface.json", "Continuar");
            }
            IsBusy = false;
        }
        
    }
}
