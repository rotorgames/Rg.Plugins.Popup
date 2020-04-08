using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.MacOS.Extensions;
using Rg.Plugins.Popup.MacOS.Impl;
using Rg.Plugins.Popup.MacOS.Platform;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using XFPlatform = Xamarin.Forms.Platform.MacOS.Platform;

[assembly: Dependency(typeof(PopupPlatformMacOS))]
namespace Rg.Plugins.Popup.MacOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformMacOS : IPopupPlatform
    {
        // It's necessary because GC in Xamarin.iOS 13 removes all UIWindow if there are not any references to them. See #459
        readonly List<NSWindow> _windows = new List<NSWindow>();

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

            //if (NSApplication.SharedApplication.KeyWindow.WindowLevel == UIWindowLevel.Normal)
            //    NSApplication.SharedApplication.KeyWindow.WindowLevel = -1;

            var renderer = page.GetOrCreateRenderer();

            var window = new PopupWindow();

            //if (IsiOS13OrNewer)
            //    _windows.Add(window);

            //window.BackgroundColor = Color.Transparent.ToUIColor();
            //window.RootViewController = new PopupPlatformRenderer(renderer);
            //window.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            //window.WindowLevel = UIWindowLevel.Normal;
            //window.MakeKeyAndVisible();

            //if (!IsiOS9OrNewer)
            //    window.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            //await window.RootViewController.PresentViewControllerAsync(renderer.ViewController, false);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            var renderer = XFPlatform.GetRenderer(page);
            var viewController = renderer?.ViewController;

            await Task.Delay(50);

            page.DescendantRemoved -= HandleChildRemoved;

            if (renderer != null && viewController != null)
            {
                var window = viewController.View.Window;
                //await window.RootViewController.DismissViewControllerAsync(false);
                DisposeModelAndChildrenRenderers(page);
                //window.RootViewController.Dispose();
                //window.RootViewController = null;
                page.Parent = null;
                //window.Hidden = true;

                //if (IsiOS13OrNewer && _windows.Contains(window))
                //    _windows.Remove(window);

                window.Dispose();
                window = null;

                //if (UIApplication.SharedApplication.KeyWindow.WindowLevel == -1)
                //    UIApplication.SharedApplication.KeyWindow.WindowLevel = UIWindowLevel.Normal;
            }
        }

        void DisposeModelAndChildrenRenderers(VisualElement view)
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

        void HandleChildRemoved(object sender, ElementEventArgs e)
        {
            var view = e.Element;
            DisposeModelAndChildrenRenderers((VisualElement)view);
        }
    }
}
