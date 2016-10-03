using System;
using System.Collections.Generic;
using System.Reflection;
using Rg.Plugins.Popup.Windows.Renderers;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms.Internals;

namespace Rg.Plugins.Popup.Windows
{
    [Preserve(AllMembers = true)]
    public static class Popup
    {
        public static void Init()
        {
            Xamarin.Forms.DependencyService.Register<PopupNavigationWinPhone>();
            Xamarin.Forms.DependencyService.Register<ScreenHelperWinPhone>();
            Xamarin.Forms.DependencyService.Register<PopupPageRenderer>();
        }
    }
}
