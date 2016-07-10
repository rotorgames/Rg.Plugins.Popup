using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.IOS
{
    [Preserve(AllMembers = true)]
    public static class Popup
    {
        internal static readonly List<Type> LinkList = new List<Type>();
        public static void Init()
        {
            // Fix https://github.com/rotorgames/Rg.Plugins.Popup/issues/9 
            // and https://github.com/rotorgames/Rg.Plugins.Popup/issues/3
            LinkList.Add(typeof(PopupNavigationIOS));
            LinkList.Add(typeof(ScreenHelperIos));
            LinkList.Add(typeof(PopupPageRenderer));
        }
    }
}
