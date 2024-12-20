using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Services;
using ToDo_CostaRica.CustomControls;
using ToDoCR.SharedDomain.JPSModels;

namespace ToDo_CostaRica.Views.Servicios.Loteria;

public partial class SorteoPopup : CustomPopup
{
    public SorteoPopup(JPSModels.DevuelvePremiosResponseAPIPremios devuelvePremiosResponseAPIPremios, int sorteo, int serie, int numero, int fracciones)
    {
        InitializeComponent();
       BindingContext = new ViewModels.Servicios.Loteria.SorteoPopupViewModel(devuelvePremiosResponseAPIPremios, serie, numero, sorteo, fracciones);
    }

    private async void Close_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}