﻿using Android.Content;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Droid.Renderers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup
{
    public static class Popup
    {
        internal static event EventHandler? OnInitialized;

        internal static bool IsInitialized { get; private set; }

        internal static Context? Context { get; private set; }

        public static void Init(Context context)
        {
            LinkAssemblies();

            Context = context;

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }

        public static bool SendBackPressed(Action? backPressedHandler = null)
        {
            var popupNavigationInstance = PopupNavigation.Instance;

            if (popupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = popupNavigationInstance.PopupStack.Last();

                var isPreventClose = lastPage.DisappearingTransactionTask != null || lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await popupNavigationInstance.RemovePageAsync(lastPage);
                    });
                }

                return true;
            }

            backPressedHandler?.Invoke();

            return false;
        }

        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
                var i = new PopupPlatformDroid();
                var r = new PopupPageRenderer(null!);
            }
        }
    }
}
