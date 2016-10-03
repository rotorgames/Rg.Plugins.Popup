using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#elif WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#endif
using Page = Xamarin.Forms.Page;

[assembly:Dependency(typeof(PopupNavigationWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    [Preserve(AllMembers = true)]
    class PopupNavigationWinPhone : IPopupNavigation
    {
        private readonly Dictionary<Page, global::Windows.UI.Xaml.Controls.Primitives.Popup> _popupPageList = new Dictionary<Page, global::Windows.UI.Xaml.Controls.Primitives.Popup>();

        [Preserve]
        public PopupNavigationWinPhone()
        {
            
        }

        public void AddPopup(PopupPage page)
        {
            var popup = new global::Windows.UI.Xaml.Controls.Primitives.Popup();
            popup.Child = page.GetOrCreateRenderer().ContainerElement;
            popup.IsOpen = true;
            page.ForceLayout();
            _popupPageList.Add(page, popup);
        }

        public void RemovePopup(PopupPage page)
        {
            var popup = _popupPageList.First(pair => pair.Key == page).Value;
            //((PopupPageRenderer)popup.Child).Dispose();
            popup.Child = null;
            popup.IsOpen = false;
            _popupPageList.Remove(page);
        }
    }
}
