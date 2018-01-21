using System;
using Rg.Plugins.Popup.Mac.Impl;
using Rg.Plugins.Popup.Mac.Renderers;

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
                var i = new PopupPlatformMac();
                var r = new PopupPageRenderer();
            }
        }
    }
}