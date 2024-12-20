
using Plugin.Maui.Audio;
using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.ViewModels.Servicios.Loteria
{
    
    public class SorteoPopupViewModel : ViewModelBase
    {
        
        private readonly IAudioManager _audioManager;
        int? sorteo;
        int? serie;
        int? numero;
        int? fracciones;
        string img;
        List<DevuelvePremios> premios;
        public string Img
        {
            get => img;
            set
            {
                SetProperty(ref img, value);
            }
        }
        public int? Sorteo
        {
            get => sorteo;
            set
            {
                SetProperty(ref sorteo, value);
            }
        }
        public int? Serie
        {
            get => serie;
            set
            {
                SetProperty(ref serie, value);
            }
        }
        public int? Numero
        {
            get => numero;
            set
            {
                SetProperty(ref numero, value);
            }
        }
        public int? Fracciones
        {
            get => fracciones;
            set
            {
                SetProperty(ref fracciones, value);
            }
        }

        public List<DevuelvePremios> Premios
        {
            get => premios;
            set
            {
                SetProperty(ref premios, value);
            }
        }

        public SorteoPopupViewModel(DevuelvePremiosResponseAPIPremios devuelvePremiosResponseAPIPremios, IAudioManager audioManager,int serie, int numero, int sorteo, int fracciones)
        {
            Premios = devuelvePremiosResponseAPIPremios.Result;
            Serie = serie;
            Numero = numero;
            Sorteo = sorteo;
            Fracciones = fracciones;
            
            _audioManager = audioManager;
            // Calling the async method to handle audio logic
            HandleAudioAndImageAsync(devuelvePremiosResponseAPIPremios).ConfigureAwait(false);

            
        }
        private async Task HandleAudioAndImageAsync(DevuelvePremiosResponseAPIPremios devuelvePremiosResponseAPIPremios)
        {
            if (devuelvePremiosResponseAPIPremios.Result.Any())
            {
               

                if (devuelvePremiosResponseAPIPremios.Result.Any(p => p.SubPremio == 0 && (p.TipoPremio == 1 || p.TipoPremio == 2 || p.TipoPremio == 3)))
                {
                    var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win_big.wa"));
                    player.Play();
                }
                else if (devuelvePremiosResponseAPIPremios.Result.Sum(p => (p.MontoUnitario * Fracciones)) >= 1000000)
                {
                    var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win_medium.wav"));
                    player.Play();
                }
                else
                {
                    var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win_small.wav"));
                    player.Play();
                }

                Img = "win.png";
            }
            else
            {
                Img = "happiness.png";
            }
        }
    }
}
