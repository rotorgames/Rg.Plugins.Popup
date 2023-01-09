﻿
using CoreGraphics;

using Foundation;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Exceptions;
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
        // It's necessary because GC in Xamarin.iOS 13 removes all UIWindow if there are not any references to them. See #459
        private readonly List<UIWindow> _windows = new List<UIWindow>();

        private static bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        private static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            page.DescendantRemoved += HandleChildRemoved;

            var keyWindow = UIApplication.SharedApplication.GetKeyWindow();
            if (keyWindow?.WindowLevel == UIWindowLevel.Normal)
                keyWindow.WindowLevel = -1;

            var renderer = page.GetOrCreateRenderer();

            PopupWindow window;
            if (IsiOS13OrNewer)
            {
                var connectedWindowScene = UIApplication.SharedApplication
                                                        .ConnectedScenes
                                                        .ToArray()
                                                        .FirstOrDefault(scene =>
                                                        {
                                                            // The popup should only be displayed on a scene that displays interactive windows
                                                            // on the device’s built-in display or an externally connected display.
                                                            if (scene.Session?.Role != UIWindowSceneSessionRole.Application)
                                                            {
                                                                return false;
                                                            }

                                                            return scene.ActivationState == UISceneActivationState.ForegroundActive
                                                                    && scene is UIWindowScene;
                                                        }) as UIWindowScene;

                if (connectedWindowScene != null)
                    window = new PopupWindow(connectedWindowScene);
                else
                    window = new PopupWindow();

                _windows.Add(window);
            }
            else
                window = new PopupWindow();

            window.BackgroundColor = Color.Transparent.ToUIColor();
            window.RootViewController = new PopupPlatformRenderer(renderer);
            if (window.RootViewController.View != null)
                window.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            window.WindowLevel = UIWindowLevel.Normal;
            window.MakeKeyAndVisible();

            if (!IsiOS9OrNewer)
                window.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            return window.RootViewController.PresentViewControllerAsync(renderer.ViewController, false);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var renderer = XFPlatform.GetRenderer(page);
            var viewController = renderer?.ViewController;

            await Task.Delay(50);

            page.DescendantRemoved -= HandleChildRemoved;

            if (renderer != null && viewController != null && !viewController.IsBeingDismissed)
            {
                var window = viewController.View?.Window;
                page.Parent = null;
                if (window != null)
                {
                    var rvc = window.RootViewController;
                    if (rvc != null)
                    {
                        await rvc.DismissViewControllerAsync(false);
                        DisposeModelAndChildrenRenderers(page);
                        rvc.Dispose();
                    }
                    window.RootViewController = null;
                    window.Hidden = true;
                    if (IsiOS13OrNewer && _windows.Contains(window))
                        _windows.Remove(window);
                    window.Dispose();
                    window = null;
                }

                var keyWindow = UIApplication.SharedApplication.GetKeyWindow();
                if (_windows.Count > 0)
                    _windows.Last().WindowLevel = UIWindowLevel.Normal;
                else if (keyWindow?.WindowLevel == -1)
                    keyWindow.WindowLevel = UIWindowLevel.Normal;
            }
        }

        private static void DisposeModelAndChildrenRenderers(VisualElement view)
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
            DisposeModelAndChildrenRenderers((VisualElement)view);
        }
    }
}
