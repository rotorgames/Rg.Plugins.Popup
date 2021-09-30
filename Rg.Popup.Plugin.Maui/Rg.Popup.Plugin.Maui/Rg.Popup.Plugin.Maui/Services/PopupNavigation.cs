using System;
using System.ComponentModel;
using Rg.Plugins.Popup.Contracts;

namespace Rg.Plugins.Popup.Services
{/*
    public static class PopupNavigation
    {
        const string DepractedMethodsText =
            "You should use "
            + nameof(IPopupNavigation) +
            " instance from "
            + nameof(PopupNavigation) +
            "."
            + nameof(Instance) +
            ".\nSee more info: "
            + Config.MigrationV1_0_xToV1_1_xUrl;

        static IPopupNavigation _popupNavigation;
        static IPopupNavigation _customNavigation;

        public static IPopupNavigation Instance
        {
            get
            {
                if (_customNavigation != null)
                    return _customNavigation;

                if (_popupNavigation == null)
                    _popupNavigation = new PopupNavigationImpl();

                return _popupNavigation;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetInstance(IPopupNavigation instance)
        {
            _customNavigation = instance;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void RestoreDefaultInstance()
        {
            _customNavigation = null;
        }
    }
    */

    public static class PopupNavigation
    {
        static IPopupNavigation _customNavigation;
        static Lazy<IPopupNavigation> implementation = new(() => CreatePopupNavigation(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value != null;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IPopupNavigation Instance
        {
            get
            {
                IPopupNavigation lazyEvalPopupNavigation = _customNavigation ?? implementation.Value;
                if (lazyEvalPopupNavigation == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return lazyEvalPopupNavigation;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetInstance(IPopupNavigation instance)
        {
            _customNavigation = instance;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void RestoreDefaultInstance()
        {
            _customNavigation = null;
        }

        static IPopupNavigation CreatePopupNavigation()
        {
            return new PopupNavigationImpl();
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }
}


