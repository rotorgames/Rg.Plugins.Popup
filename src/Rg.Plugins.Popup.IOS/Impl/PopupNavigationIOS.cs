using System.Collections.Generic;
using System.Collections.ObjectModel;
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

[assembly: Dependency(typeof(PopupNavigationIOS))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupNavigationIOS : IPopupNavigation
    {
        public void AddPopup(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var renderer = page.GetOrCreateRenderer();

            var windows = new UIWindow
            {
                BackgroundColor = Color.Transparent.ToUIColor()
            };
            windows.RootViewController = new PopupPlatformRenderer(renderer);
            windows.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            windows.WindowLevel = UIWindowLevel.Normal;
            windows.MakeKeyAndVisible();

            windows.RootViewController.PresentViewController(renderer.ViewController, false, null);
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
            foreach (VisualElement child in GetElementDescendants(view))
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

        private IEnumerable<Element> GetElementDescendants(Element element)
        {
            var queue = new Queue<Element>(16);
            queue.Enqueue(element);

            while (queue.Count > 0)
            {
                ReadOnlyCollection<Element> children = ((IElementController)queue.Dequeue()).LogicalChildren;
                for (var i = 0; i < children.Count; i++)
                {
                    Element child = children[i];
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }
    }
}
