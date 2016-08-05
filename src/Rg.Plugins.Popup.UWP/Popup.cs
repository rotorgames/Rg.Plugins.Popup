using System;
using System.Collections.Generic;
using System.Reflection;
using Rg.Plugins.Popup.WinPhone.Impl;
using Rg.Plugins.Popup.WinPhone.Renderers;

namespace Rg.Plugins.Popup.UWP
{
    [Preserve(AllMembers = true)]
    public static class Popup
    {
        public static IList<Assembly> GetLinkedAssemblies(IList<Assembly> userAssemblies = null)
        {
            // Fix: https://github.com/rotorgames/Rg.Plugins.Popup/issues/31
            var assemblies = userAssemblies ?? new List<Assembly>();
            assemblies.Add(typeof(PopupNavigationWinPhone).GetTypeInfo().Assembly);
            assemblies.Add(typeof(ScreenHelperWinPhone).GetTypeInfo().Assembly);
            assemblies.Add(typeof(PopupPageRenderer).GetTypeInfo().Assembly);

            return assemblies;
        }
    }
}
