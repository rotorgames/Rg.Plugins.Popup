using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Rg.Plugins.Popup.Droid.Helpers
{
    internal static class PlatformHelper
    {
        #region Navigation Methods

        public static Delegate GetHandleBackPressed<TDelegate>()
        {
            var platform = GetPlatform();

            var handleBackPressed = platform.GetType()
                .GetMethod("HandleBackPressed", BindingFlags.NonPublic | BindingFlags.Instance)
                .CreateDelegate(typeof (TDelegate), platform);

            return handleBackPressed;
        }

        #endregion

        #region Reflection Private Methods

        private static object GetPlatform()
        {
            object platform = null;

            if (Forms.Context is FormsApplicationActivity)
            {
                var activityType = typeof(FormsApplicationActivity);
                platform = activityType.GetField("_canvas", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Forms.Context);
            }
            else if(Forms.Context is FormsAppCompatActivity)
            {
                var activityType = typeof(FormsAppCompatActivity);
                platform = activityType.GetField("_platform", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Forms.Context);
            }

            if (platform == null)
            {
                throw new InvalidOperationException("Platform is not created");
            }

            return platform;
        }

        #endregion
    }
}