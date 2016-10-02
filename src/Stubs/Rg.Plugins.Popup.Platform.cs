using System;
#if __ANDROID__
using Rg.Plugins.Popup.Droid.Renderers;
#elif __IOS__
using Rg.Plugins.Popup.IOS.Renderers;
#elif __WIN_PHONE__ || __UWP__
using Rg.Plugins.Popup.WinPhone.Renderers;
#endif
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Platform
{
#if !__PLATFORM_PCL__
    [RenderWith(PopupPageRenderer)]
#endif
    internal class _PopupPageRenderer { }
}
