using System;
using System.Collections.Generic;
using System.Reflection;
using Rg.Plugins.Popup.Windows.Renderers;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms.Internals;

namespace Rg.Plugins.Popup.Windows
{
    [Preserve(AllMembers = true)]
    public static class Popup
    {
#if WINDOWS_UWP
        /// <summary>
        /// Use this method for UWP project .NET Native compilation and add result to <see cref="T:Xamarin.Forms.Forms.Init"/>
        /// </summary>
        /// <param name="defaultAssemblies">Custom assemblies from other libs or your DI implementations and renderers</param>
        /// <returns>All assemblies for <see cref="T:Xamarin.Forms.Forms.Init"/></returns>
        public static IEnumerable<Assembly> GetExtraAssemblies(IEnumerable<Assembly> defaultAssemblies = null)
        {
            var assemblies = new List<Assembly>
            {
                GetAssembly<PopupPlatformWinPhone>(),
                GetAssembly<PopupPageRenderer>()
            };

            if(defaultAssemblies != null)
                assemblies.AddRange(defaultAssemblies);

            return assemblies;
        }

        private static Assembly GetAssembly<T>()
        {
            return typeof (T).GetTypeInfo().Assembly;
        }

        [Obsolete("Use GetExtraAssemblies for UWP project", true)]
#else
        [Obsolete("Initialization is not required in UWP and WP projects")]
#endif
        public static void Init()
        {
        }
    }
}
