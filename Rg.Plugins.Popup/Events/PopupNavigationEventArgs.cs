using System;
using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Events
{
    public class PopupNavigationEventArgs : EventArgs
    {
        public PopupPage Page { get; }

        public bool IsAnimated { get; }

        public PopupNavigationEventArgs(PopupPage page, bool isAnimated)
        {
            Page = page;
            IsAnimated = isAnimated;
        }
    }
}
