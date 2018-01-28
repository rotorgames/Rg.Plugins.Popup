using System;

#if __IOS__
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.IOS.Renderers;
#elif __MACOS__
using Rg.Plugins.Popup.Mac.Impl;
using Rg.Plugins.Popup.Mac.Renderers;
#endif

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
#if __IOS__
                var i = new PopupPlatformIos();
#elif __MACOS__
                var i = new PopupPlatformMac();
#endif
                var r = new PopupPageRenderer();
            }
        }
    }
}