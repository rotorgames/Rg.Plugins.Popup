using Rg.Plugins.Popup.Tizen.Impl;
using Rg.Plugins.Popup.Tizen.Renderers;
using System;

namespace Rg.Plugins.Popup.Tizen
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
                var i = new PopupPlatformTizen();
                var r = new PopupPageRenderer();
            }
        }
    }
}