using System.Linq;

using Rg.Plugins.Popup.IOS.Renderers;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;

namespace Rg.Plugins.Popup.IOS.Extensions
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
            var applicationFrame = UIScreen.MainScreen.ApplicationFrame;

            var systemPadding = new Thickness
            {
                Left = applicationFrame.Left,
                Top = applicationFrame.Top,
                Right = applicationFrame.Right - applicationFrame.Width - applicationFrame.Left,
                Bottom = applicationFrame.Bottom - applicationFrame.Height - applicationFrame.Top + renderer.KeyboardBounds.Height
            };

            if ((renderer.Element.Width != superviewFrame.Width && renderer.Element.Height != superviewFrame.Height)
                || currentElement.SystemPadding.Bottom != systemPadding.Bottom)
            {
                currentElement.BatchBegin();
                currentElement.SystemPadding = systemPadding;
                renderer.SetElementSize(new Size(superviewFrame.Width, superviewFrame.Height));
                currentElement.BatchCommit();
            }
        }
    }
}
