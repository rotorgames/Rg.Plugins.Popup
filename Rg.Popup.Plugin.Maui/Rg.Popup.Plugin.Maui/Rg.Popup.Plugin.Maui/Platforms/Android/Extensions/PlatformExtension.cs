

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Droid.Extensions
{
    internal static class PlatformExtension
    {

        public static IViewHandler GetOrCreateHandler(this VisualElement bindable)
        {
            try
            {
                var handler = bindable.Handler;
                if (handler == null)
                {
                    handler = new PopupPageHandler();
                }

                bindable.Handler = handler;
                return handler;
            }
            catch (System.Exception ex)
            {

                throw;
            }


        }
        /*
        public static Microsoft.Maui.Controls.Compatibility.Platform.Android.IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = Platform.GetRenderer(bindable);
            if (renderer == null)
            {
                Platform.
                renderer = Platform.CreateRendererWithContext(bindable, Microsoft.Maui.Essentials.Platform.AppContext);
                Platform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }
        */
    }
}
