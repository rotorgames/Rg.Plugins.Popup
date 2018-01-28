using System.Linq;
using Xamarin.Forms;

#if __IOS__
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Rg.Plugins.Popup.IOS.Renderers;
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;
#elif __MACOS__
using AppKit;
using Xamarin.Forms.Platform.MacOS;
using Rg.Plugins.Popup.Mac.Renderers;
using XFPlatform = Xamarin.Forms.Platform.MacOS.Platform;
#endif

#if __IOS__
namespace Rg.Plugins.Popup.IOS.Extensions
#elif __MACOS__
namespace Rg.Plugins.Popup.Mac.Extensions
#endif
{
    internal static class PlatformExtension
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = XFPlatform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = XFPlatform.CreateRenderer(bindable);
                XFPlatform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }

        public static void DisposeModelAndChildrenRenderers(this VisualElement view)
        {
            IVisualElementRenderer renderer;
            foreach (var child in view.Descendants().OfType<VisualElement>())
            {
                renderer = XFPlatform.GetRenderer(child);
                XFPlatform.SetRenderer(child, null);

                if (renderer == null)
                    continue;

                renderer.NativeView.RemoveFromSuperview();
                renderer.Dispose();
            }

            renderer = XFPlatform.GetRenderer(view);
            if (renderer != null)
            {
                renderer.NativeView.RemoveFromSuperview();
                renderer.Dispose();
            }
            XFPlatform.SetRenderer(view, null);
        }

        public static void UpdateSize(this PopupPageRenderer renderer)
        {
            var currentElement = renderer.CurrentElement;

            if (renderer.View?.Superview?.Frame == null || currentElement == null)
                return;

            var superviewFrame = renderer.View.Superview.Frame;
#if __IOS__
            var applactionFrame = UIScreen.MainScreen.ApplicationFrame;
#elif __MACOS__
            var applactionFrame = NSScreen.MainScreen.Frame;
#endif
            var systemPadding = new Thickness
            {
                Left = applactionFrame.Left,
                Top = applactionFrame.Top,
                Right = applactionFrame.Right - applactionFrame.Width - applactionFrame.Left,
#if __IOS__
                Bottom = applactionFrame.Bottom - applactionFrame.Height - applactionFrame.Top + renderer.KeyboardBounds.Height
#elif __MACOS__
                Bottom = applactionFrame.Bottom - applactionFrame.Height - applactionFrame.Top
#endif
            };

            currentElement.BatchBegin();

            currentElement.SetSystemPadding(systemPadding);
            renderer.SetElementSize(new Size(superviewFrame.Width, superviewFrame.Height));

            currentElement.BatchCommit();
        }
    }
}