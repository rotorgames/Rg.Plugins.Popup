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
        #region Renderers Methods

        public static IVisualElementRenderer GetRenderer(BindableObject bindable)
        {
            var type = CreatePlatformType();

            var method = type.GetMethod("GetRenderer", BindingFlags.Public | BindingFlags.Static);

            return (IVisualElementRenderer)method.Invoke(null, new Object[] { bindable });
        }

        public static void SetRenderer(BindableObject bindable, IVisualElementRenderer value)
        {
            var type = CreatePlatformType();

            var method = type.GetMethod("SetRenderer", BindingFlags.Public | BindingFlags.Static);

            method.Invoke(null, new Object[] { bindable, value });
        }

        public static IVisualElementRenderer CreateRenderer(BindableObject bindable)
        {
            var type = CreatePlatformType();

            var method = type.GetMethod("CreateRenderer", BindingFlags.Public | BindingFlags.Static);

            return (IVisualElementRenderer)method.Invoke(null, new Object[] { bindable });
        }

        public static BindableProperty RendererProperty
        {
            get
            {
                var type = CreatePlatformType();

                var property = type.GetRuntimeFields().First(p => p.Name == "RendererProperty");
                return (BindableProperty)property.GetValue(null);
            }
        }

        #endregion

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

        private static Type CreatePlatformType()
        {
            var assembly = Assembly.Load("Xamarin.Forms.Platform.Android");
            return assembly.GetType("Xamarin.Forms.Platform.Android.Platform");
        }

        private static object GetPlatform()
        {
            object platform = null;

            if (Forms.Context is FormsApplicationActivity)
            {
                var activityType = typeof(FormsApplicationActivity);
                platform = activityType.GetField("canvas", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Forms.Context);
            }
            else if(Forms.Context is FormsAppCompatActivity)
            {
                var activityType = typeof(FormsAppCompatActivity);
                platform = activityType.GetField("platform", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Forms.Context);
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