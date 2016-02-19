using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Rg.Plugins.Popup.IOS.Helpers
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

        #region Reflection Private Methods

        private static Type CreatePlatformType()
        {
            var assembly = Assembly.Load("Xamarin.Forms.Platform.iOS");
            return assembly.GetType("Xamarin.Forms.Platform.iOS.Platform");
        }

        #endregion
    }
}