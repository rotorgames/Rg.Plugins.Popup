using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Services
{
    public static class PopupNavigation
    {
        private const string DepractedMethodsText = 
            "You should use "
            +nameof(IPopupNavigation)+
            " instance from "
            +nameof(PopupNavigation)+
            "."
            +nameof(Instance)+
            ".\nSee more info: "
            +Config.MigrationV1_0_xToV1_1_xUrl;
        
        private static IPopupNavigation _popupNavigation;

        public static IPopupNavigation Instance
        {
            get
            {
                if(_popupNavigation == null)
                    _popupNavigation = new PopupNavigationImpl();

                return _popupNavigation;
            }
        }

        [Obsolete(DepractedMethodsText)]
        public static IReadOnlyList<PopupPage> PopupStack => Instance.PopupStack;

        [Obsolete(DepractedMethodsText)]
        public static Task PushAsync(PopupPage page, bool animate = true)
        {
            return Instance.PushAsync(page, animate);
        }

        [Obsolete(DepractedMethodsText)]
        public static Task PopAsync(bool animate = true)
        {
            return Instance.PopAsync(animate);
        }

        [Obsolete(DepractedMethodsText)]
        public static Task PopAllAsync(bool animate = true)
        {
            return Instance.PopAllAsync(animate);
        }

        [Obsolete(DepractedMethodsText)]
        public static Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            return Instance.RemovePageAsync(page, animate);
        }
    }
}
