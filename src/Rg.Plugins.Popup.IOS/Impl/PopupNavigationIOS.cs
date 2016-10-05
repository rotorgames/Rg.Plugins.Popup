using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.IOS.Extensions;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(PopupNavigationIOS))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupNavigationIOS : IPopupNavigation
    {
        public void AddPopup(PopupPage page)
        {
            var renderer = page.GetOrCreateRenderer();
            GetTopViewController().View.AddSubview(renderer.NativeView);
        }

        public void RemovePopup(PopupPage page)
        {
            var renderer = page.GetOrCreateRenderer();
            var viewController = renderer?.ViewController;

            if (viewController != null && !viewController.IsBeingDismissed)
                renderer.NativeView.RemoveFromSuperview();

        }

        private UIViewController GetTopViewController()
        {
            var navigation = Application.Current.MainPage.Navigation;

            if (navigation.ModalStack.Count > 0)
                return navigation.ModalStack.First().GetOrCreateRenderer().ViewController;

            return UIApplication.SharedApplication.KeyWindow.RootViewController;
        }
    }
}
