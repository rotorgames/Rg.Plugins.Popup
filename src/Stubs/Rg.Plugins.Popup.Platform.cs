using System;
#if __ANDROID__
using Rg.Plugins.Popup.Droid.Renderers;
#elif __IOS__
using Rg.Plugins.Popup.IOS.Renderers;
#elif __WP__ || __UWP__
using Rg.Plugins.Popup.Windows.Renderers;
#endif
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Platform.Renderers
{
#if !__PLATFORM_PCL__
    [RenderWith(typeof(PopupPageRenderer))]
#endif
    public class _PopupPageRenderer { }
}
