using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Rg.Plugins.Popup.IOS.Extensions
{
    internal static class PlatformExtension
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = Platform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(bindable);
                Platform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }
    }
}
