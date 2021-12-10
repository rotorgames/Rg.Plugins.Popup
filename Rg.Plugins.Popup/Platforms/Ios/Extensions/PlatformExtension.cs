using System.Linq;

using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.Pages;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;

namespace Rg.Plugins.Popup.IOS.Extensions
{
    internal static class PlatformExtension
    {
        private static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

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
            var keyboardOffset = renderer.KeyboardBounds.Height;

            Thickness systemPadding;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
                && UIApplication.SharedApplication.KeyWindow != null)
            {
                var safeAreaInsets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;

                systemPadding = new Thickness(
                    safeAreaInsets.Left,
                    safeAreaInsets.Top,
                    safeAreaInsets.Right,
                    safeAreaInsets.Bottom);
            }
            else
            {
                systemPadding = new Thickness
                {
                    Left = applicationFrame.Left,
                    Top = applicationFrame.Top,
                    Right = applicationFrame.Right - applicationFrame.Width - applicationFrame.Left,
                    Bottom = applicationFrame.Bottom - applicationFrame.Height - applicationFrame.Top
                };
            }

            var needForceLayout =
                (currentElement.HasSystemPadding && currentElement.SystemPadding != systemPadding)
                || (currentElement.HasKeyboardOffset && currentElement.KeyboardOffset != keyboardOffset);

            currentElement.SetValueFromRenderer(PopupPage.SystemPaddingProperty, systemPadding);
            currentElement.SetValueFromRenderer(PopupPage.KeyboardOffsetProperty, keyboardOffset);

            var elementSize = new Size(superviewFrame.Width, superviewFrame.Height);

            if (currentElement.Bounds.Size != elementSize)
                renderer.SetElementSize(elementSize);
            else if (needForceLayout)
                currentElement.ForceLayout();
        }

        public static UIWindow GetKeyWindow(this UIApplication application)
        {
            if (!IsiOS13OrNewer)
                return UIApplication.SharedApplication.KeyWindow;

            var window = application
                .ConnectedScenes
                .ToArray()
                .OfType<UIWindowScene>()
                .SelectMany(scene => scene.Windows)
                .FirstOrDefault(window => window.IsKeyWindow);

            return window;
        }
    }
}
