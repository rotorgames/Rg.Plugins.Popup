using Demo.iOS;
using Demo.Pages;
using Rg.Plugins.Popup.IOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LoginPopupPage), typeof(LoginPopupPageRenderer))]
namespace Demo.iOS
{
    public class LoginPopupPageRenderer : PopupPageRenderer
    { 
        public override bool PrefersStatusBarHidden()
        {
            return false; 
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                return UIStatusBarStyle.DarkContent;
            }
            return UIStatusBarStyle.LightContent; 
        }
    }
}
