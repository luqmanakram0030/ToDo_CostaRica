using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Services;
using ToDo_CostaRica.CustomControls;
using ToDo_CostaRica.ViewModels;

namespace ToDo_CostaRica.Views;

public partial class CustomPopupMessage : CustomPopup
{
    public CustomPopupMessage(CustomPopupMessageViewModel customPopupMessageViewModel)
    {
        InitializeComponent();
        _ = Task.Run(async () =>
        {
            await Task.Delay(250);
            Device.BeginInvokeOnMainThread(() =>
            {
                if (customPopupMessageViewModel.Image.Contains("json"))
                {
                    Lottie.IsVisible = true;
                }
                else
                {
                    Img.IsVisible = true;
                }
            });
        });
        BindingContext = customPopupMessageViewModel;
    }

    private async void Close_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}