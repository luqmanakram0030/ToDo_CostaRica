using Mopups.Pages;
using System.Threading.Tasks;

namespace ToDo_CostaRica.CustomControls
{
    public partial class CustomPopup : PopupPage
    {
        public CustomPopup()
        {
            // Initialize your popup here (e.g., binding context, UI setup, etc.)
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Custom logic for when the popup appears
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Custom logic for when the popup disappears
        }

        // Animations
        protected override async Task OnAppearingAnimationBeginAsync()
        {
            await base.OnAppearingAnimationBeginAsync();
            // Add custom appearing animation logic
        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            await base.OnDisappearingAnimationBeginAsync();
            // Add custom disappearing animation logic
        }

        // Prevent Closing Popup
        protected override bool OnBackButtonPressed()
        {
            // Return true to prevent closing with back button
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false to prevent closing by clicking the background
            return false;
        }
    }
}