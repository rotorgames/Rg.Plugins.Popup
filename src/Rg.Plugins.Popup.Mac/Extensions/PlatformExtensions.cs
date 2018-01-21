using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using XFPlatform = Xamarin.Forms.Platform.MacOS.Platform;

namespace Rg.Plugins.Popup.Mac.Extensions
{
    internal static class PlatformExtensions
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
    }
}