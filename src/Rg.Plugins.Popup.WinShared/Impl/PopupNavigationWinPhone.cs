using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.WinPhone.Impl;
using Rg.Plugins.Popup.WinPhone.Renderers;
using Xamarin.Forms;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#elif WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#endif
using Page = Xamarin.Forms.Page;

[assembly:Dependency(typeof(PopupNavigationWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    class PopupNavigationWinPhone : IPopupNavigation
    {
        private readonly Dictionary<Page, Windows.UI.Xaml.Controls.Primitives.Popup> _popupPageList = new Dictionary<Page, Windows.UI.Xaml.Controls.Primitives.Popup>(); 

        public void AddPopup(PopupPage page)
        {
            var popup = new Windows.UI.Xaml.Controls.Primitives.Popup();
            popup.Child = page.GetOrCreateRenderer().ContainerElement;
            popup.IsOpen = true;
            page.ForceLayout();
            _popupPageList.Add(page, popup);
        }

        public void RemovePopup(PopupPage page)
        {
            var popup = _popupPageList.First(pair => pair.Key == page).Value;
            ((PopupPageRenderer)popup.Child).Destroy();
            popup.Child = null;
            popup.IsOpen = false;
            _popupPageList.Remove(page);
        }
    }
}
