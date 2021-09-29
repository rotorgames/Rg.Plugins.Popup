

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;


namespace Rg.Plugins.Popup.Droid.Extensions
{
    internal static class PlatformExtension
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = Platform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = Platform.CreateRendererWithContext(bindable, Microsoft.Maui.Essentials.Platform.AppContext);
                Platform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }
    }
}