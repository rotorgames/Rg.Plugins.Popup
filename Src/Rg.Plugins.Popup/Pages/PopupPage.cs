using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Pages
{
    public class PopupPage : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.PopAsync();
            return true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Device.OS == TargetPlatform.Android)
            {
                Layout(DependencyService.Get<IScreenHelper>().ScreenSize);
            }
        }
    }
}
