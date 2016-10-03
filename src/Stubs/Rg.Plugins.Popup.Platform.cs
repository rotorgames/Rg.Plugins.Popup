using System;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Platform.Renderers
{
#if !__PLATFORM_PCL__
    [RenderWith(typeof(PopupPageRenderer))]
#endif
    public class _PopupPageRenderer { }
}
