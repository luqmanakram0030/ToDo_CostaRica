using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Services;
using ToDo_CostaRica.CustomControls;

namespace ToDo_CostaRica.Views.Settings;

public partial class AgregarOpinionPopup : CustomPopup
{
    public AgregarOpinionPopup()
    {
        InitializeComponent();            
    }

    private async void Close_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}