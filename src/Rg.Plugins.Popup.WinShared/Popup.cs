using System;
using System.Collections.Generic;
using System.Reflection;
using Rg.Plugins.Popup.Windows.Renderers;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Rg.Plugins.Popup
{
    [Preserve(AllMembers = true)]
    public static class Popup
    {
        internal static event EventHandler OnInitialized;

        internal static bool IsInitialized { get; private set; }

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

#endif
        public static void Init()
        {
            LinkAssemblies();

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }

        private static void LinkAssemblies()
        {
            DependencyService.Register<PopupPlatformWinPhone>();

            if (false.Equals(true))
            {
                var r = new PopupPageRenderer();
            }
        }
    }
}
