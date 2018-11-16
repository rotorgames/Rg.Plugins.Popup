using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.Contracts;
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

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            page.DescendantRemoved += HandleChildRemoved;

            var renderer = page.GetOrCreateRenderer();

            var window = new PopupWindow
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

            await window.RootViewController.PresentViewControllerAsync(renderer.ViewController, false);
            await Task.Delay(5);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            var renderer = XFPlatform.GetRenderer(page);
            var viewController = renderer?.ViewController;

            await Task.Delay(50);

            page.DescendantRemoved -= HandleChildRemoved;

            if (renderer != null && viewController != null && !viewController.IsBeingDismissed)
            {
                var window = viewController.View.Window;
                await window.RootViewController.DismissViewControllerAsync(false);
                DisposeModelAndChildrenRenderers(page);
                window.RootViewController.Dispose();
                window.RootViewController = null;
                page.Parent = null;
                window.Hidden = true;
            }

            await Task.Delay(5);
        }

        private void DisposeModelAndChildrenRenderers(VisualElement view)
        {
            IVisualElementRenderer renderer;
            foreach (VisualElement child in view.Descendants())
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

        private void HandleChildRemoved(object sender, ElementEventArgs e)
        {
            var view = e.Element;
            DisposeModelAndChildrenRenderers((VisualElement) view);
        }
    }
}
