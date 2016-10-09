using System;
#if __ANDROID__
using Rg.Plugins.Popup.Droid.Renderers;
#elif __IOS__
using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.IOS.Impl;
#endif
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Platform.Renderers
{
    internal static class Loader
    {
        public static void Load()
        {
#if __IOS__
            new PopupPageRenderer();
#endif
        }
    }

#if !__PLATFORM_PCL__
    [RenderWith(typeof(PopupPageRenderer))]
#endif
    internal class _PopupPageRenderer { }
}
