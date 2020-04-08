
using System;
using Rg.Plugins.Popup.MacOS.Impl;
using Rg.Plugins.Popup.MacOS.Renderers;

namespace Rg.Plugins.Popup
{
    public static class Popup
    {
        internal static event EventHandler OnInitialized;

        internal static bool IsInitialized { get; private set; }

        public static void Init()
        {
            LinkAssemblies();

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }

        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
                var i = new PopupPlatformMacOS();
                var r = new PopupPageRenderer();
            }
        }
    }
}
