using Akavache;

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.MauiMTAdmob;
using ToDoCR.SharedDomain.Helpers;

namespace ToDo_CostaRica.Infrastructure
{
    public class TodoCRAds : IDisposable
    {
        private int accionesHechas;

        private bool Mostrando { get; set; }
        private bool MostrarAhora { get; set; }
        private int EsperaAcciones { get; set; }

        private int AccionesHechas
        {
            get => accionesHechas; set
            {
                try
                {
                    BlobCache.Secure.InsertObject("AccionesHechas", value);
                }
                catch (Exception)
                {
                }
                accionesHechas = value;
            }
        }

        public TodoCRAds()
        {
            _ = Init();
        }

        private async Task Init()
        {
            try
            {
                accionesHechas = await BlobCache.Secure.GetObject<int>("AccionesHechas");
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }

            EsperaAcciones = 2;
            CrossMauiMTAdmob.Current.OnInterstitialLoaded += Current_OnInterstitialLoaded;
            CrossMauiMTAdmob.Current.OnInterstitialClosed += Current_OnInterstitialClosed;
        }

        public void Dispose()
        {
            CrossMauiMTAdmob.Current.OnInterstitialLoaded -= Current_OnInterstitialLoaded;
            CrossMauiMTAdmob.Current.OnInterstitialClosed -= Current_OnInterstitialClosed;
        }

        public void CargarInterstitial(string adId = "ca-app-pub-3940256099942544/1033173712", bool mostrar = false)
        {
            MostrarAhora = mostrar;
            if (!CrossMauiMTAdmob.Current.IsInterstitialLoaded())
                CrossMauiMTAdmob.Current.LoadInterstitial(adId);
        }

        public void MostrarInterstitial()
        {
            if (AccionesHechas == 0 && CrossMauiMTAdmob.Current.IsInterstitialLoaded())
            {
                Mostrando = true;
                CrossMauiMTAdmob.Current.ShowInterstitial();
            }
            AccionesHechas++;
            if (AccionesHechas > EsperaAcciones)
            {
                AccionesHechas = 0;
                if (!CrossMauiMTAdmob.Current.IsInterstitialLoaded())
                {
                    CargarInterstitial(mostrar: false);
                }
            }
        }

        public bool EstaInterstitialCargado()
        {
            return CrossMauiMTAdmob.Current.IsInterstitialLoaded();
        }

        internal async Task EsperarAnuncio()
        {
            await TaskEx.WaitWhile(() => Mostrando);
        }

        private void Current_OnInterstitialClosed(object sender, EventArgs e)
        {
            /// Aqui puedo usar el messaging center para notificar
            /// la pantalla actual cuando se cierra el anuncio
            /// y hacer algo especifico
            Mostrando = false;
        }

        private void Current_OnInterstitialLoaded(object sender, EventArgs e)
        {
            if (MostrarAhora)
            {
                MostrarInterstitial();
            }
        }

        internal bool VaAMostrarAnuncio()
        {
            return AccionesHechas == 0;
        }
    }
}