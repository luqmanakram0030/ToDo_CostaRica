using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoCR.SharedDomain.Models;
using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.ViewModels.Servicios.Loteria
{
    public class SorteoPopupViewModel : ViewModelBase
    {
        ISimpleAudioPlayer Player;

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

        public SorteoPopupViewModel(DevuelvePremiosResponseAPIPremios devuelvePremiosResponseAPIPremios, int serie, int numero, int sorteo, int fracciones)
        {
            Premios = devuelvePremiosResponseAPIPremios.Result;
            Serie = serie;
            Numero = numero;
            Sorteo = sorteo;
            Fracciones = fracciones;
            if (devuelvePremiosResponseAPIPremios.Result.Any())
            {
                Player = CrossSimpleAudioPlayer.Current;
                if (devuelvePremiosResponseAPIPremios.Result.Any(p => p.SubPremio == 0 && (p.TipoPremio == 1 || p.TipoPremio == 2 || p.TipoPremio == 3)))
                {
                    Player.Load("win_big.wav");
                }
                else if (devuelvePremiosResponseAPIPremios.Result.Sum(p => (p.MontoUnitario * fracciones)) >= 1000000)
                {
                    Player.Load("win_medium.wav");
                }
                else
                    Player.Load("win_small.wav");
                Player.Play();
                Img = "win.png";
            }
            else
                Img = "happiness.png";
            // 804, 60
            //Fracciones = 1;
            //if (devuelvePremiosResponseAPIPremios.Result.Any())
            //{
            //    var resultado = devuelvePremiosResponseAPIPremios?.Result.FirstOrDefault();
            //    Serie = resultado?.Serie ?? 0;
            //    Numero = resultado?.Numero ?? 0;
            //    Sorteo = resultado?.NumeroSorteo ?? 0;

            //    if (resultado.TipoPremio >= 0)
            //    {
            //        if (resultado.TipoPremio == 3)
            //            Mensaje.Add("Ha sido ganador del tercer premio.");
            //        else if (resultado.TipoPremio == 2)
            //            Mensaje.Add("Ha sido ganador del segundo premio.");
            //        else if (resultado.TipoPremio == 1)
            //            Mensaje.Add("Ha sido ganador del primer premio.");
            //        else
            //            Mensaje.Add(resultado.Descripcion);
            //        MontoGanado.Add((resultado.MontoUnitario * fracciones)?.ToString("N0"));
            //    }

            //}
            //else
            //{
            //    Mensaje.Add("No ha resultado ganador con ese número y serie. Gracias por participar y colaborar con la Junta de Protección Social, siga ayudando a la JPS.");
            //}
        }

    }
}
