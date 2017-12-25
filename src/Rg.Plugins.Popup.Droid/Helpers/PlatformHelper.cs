using System;
using System.Reflection;
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

            if (Popup.Context is FormsApplicationActivity)
            {
                var activityType = typeof(FormsApplicationActivity);
                platform = activityType.GetField("_canvas", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Popup.Context);
            }
            else if(Popup.Context is FormsAppCompatActivity)
            {
                var activityType = typeof(FormsAppCompatActivity);
                platform = activityType.GetField("_platform", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Popup.Context);
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