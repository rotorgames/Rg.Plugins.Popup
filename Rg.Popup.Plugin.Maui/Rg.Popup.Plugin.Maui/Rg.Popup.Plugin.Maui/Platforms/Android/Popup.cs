
using Android.Content;
using Microsoft.Maui.Essentials;
using Rg.Plugins.Popup.Services;

using System;

namespace Rg.Plugins.Popup
{
    public static class Popup
    {
        internal static event EventHandler OnInitialized;

        internal static bool IsInitialized { get; private set; }

        internal static Context Context { get; private set; }

        public static void Init(Context context)
        {
            //LinkAssemblies();

            Context = context;

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }

        public static bool SendBackPressed(Action backPressedHandler = null)
        {
            var popupNavigationInstance = PopupNavigation.Instance;

            if (popupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = popupNavigationInstance.PopupStack[popupNavigationInstance.PopupStack.Count - 1];

                var isPreventClose = lastPage.DisappearingTransactionTask != null || lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    Microsoft.Maui.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await popupNavigationInstance.RemovePageAsync(lastPage);
                    });
                }

                return true;
            }

            backPressedHandler?.Invoke();

            return false;
        }
        /*
        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
                _ = new PopupPlatformDroid();
                _ = new PopupPageRenderer(null);
            }
        }
        */
    }
}
