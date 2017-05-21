using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.IOS.Extensions;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.IOS.Platform;
using Rg.Plugins.Popup.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;

[assembly: Dependency(typeof(PopupPlatformIos))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformIos : IPopupPlatform
    {
        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        public void AddPopup(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var renderer = page.GetOrCreateRenderer();

            var window = new UIWindow
            {
                BackgroundColor = Color.Transparent.ToUIColor()
            };
            window.RootViewController = new PopupPlatformRenderer(renderer);
            window.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            window.WindowLevel = UIWindowLevel.Normal;
            window.MakeKeyAndVisible();

            if (!IsiOS9OrNewer)
            {
                window.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            }

            window.RootViewController.PresentViewController(renderer.ViewController, false, null);
        }

        public void RemovePopup(PopupPage page)
        {
            var renderer = XFPlatform.GetRenderer(page);
            var viewController = renderer?.ViewController;

            if(renderer == null)
                return;

            if (viewController != null && !viewController.IsBeingDismissed)
            {
                var window = viewController.View.Window;
                DisposeModelAndChildrenRenderers(page);
                window.RootViewController.DismissViewController(false, null);
                window.RootViewController.Dispose();
                window.RootViewController = null;
                page.Parent = null;
                window.Hidden = true;
            }
        }

        private void DisposeModelAndChildrenRenderers(VisualElement view)
        {
            IVisualElementRenderer renderer;
            foreach (VisualElement child in view.RgDescendants())
            {
                renderer = XFPlatform.GetRenderer(child);
                XFPlatform.SetRenderer(child, null);

                if (renderer != null)
                {
                    renderer.NativeView.RemoveFromSuperview();
                    renderer.Dispose();
                }
            }

            renderer = XFPlatform.GetRenderer(view);
            if (renderer != null)
            {
                renderer.NativeView.RemoveFromSuperview();
                renderer.Dispose();
            }
            XFPlatform.SetRenderer(view, null);
        }
    }
}
