using System;
using Android.Content;
using Android.OS;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Droid.Renderers;

namespace Rg.Plugins.Popup
{
    public static class Popup
    {
        internal static event EventHandler OnInitialized;

        internal static bool IsInitialized { get; private set; }

        internal static Context Context { get; private set; }

        public static void Init(Context context, Bundle bundle)
        {
            LinkAssemblies();

            Context = context;

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }

        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
                var i = new PopupPlatformDroid();
                var r = new PopupPageRenderer(null);
            }
        }
    }
}
