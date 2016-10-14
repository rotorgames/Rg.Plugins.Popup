using System.Linq;
using Foundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.IOS.Extensions;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.Pages;
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
            var topViewController = GetTopViewController();
            var topRenderer = topViewController.ChildViewControllers.LastOrDefault() as IVisualElementRenderer;

            if (topRenderer != null)
                page.Parent = topRenderer.Element;
            else
                page.Parent = Application.Current.MainPage;

            topViewController.View.AddSubview(renderer.NativeView);
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
            var topViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topViewController.PresentedViewController != null)
            {
                topViewController = topViewController.PresentedViewController;
            }

            return topViewController;
        }
    }
}
