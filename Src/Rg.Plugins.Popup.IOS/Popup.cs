using System;
using System.Collections.Generic;
using System.Text;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.Services;

namespace Rg.Plugins.Popup.IOS
{
    public static class Popup
    {
        [Obsolete]
        public static void Init()
        {
            // Fix sdk linker
            var temp = DateTime.Now;
            var t = typeof (PopupNavigationIOS);
            var t2 = typeof (ScreenHelperIos);
        }
    }
}
