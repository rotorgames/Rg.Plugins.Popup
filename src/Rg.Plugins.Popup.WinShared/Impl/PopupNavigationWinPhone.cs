using System.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Windows.Renderers;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Core;
#elif WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
using Xamarin.Forms.Platform.WinRT;
#endif
using Page = Xamarin.Forms.Page;

[assembly:Dependency(typeof(PopupNavigationWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    [Preserve(AllMembers = true)]
    class PopupNavigationWinPhone : IPopupNavigation
    {
        [Preserve]
        public PopupNavigationWinPhone()
        {
#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += OnBackPressed;
#elif WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
#endif
        }

#if WINDOWS_UWP
        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
#elif WINDOWS_PHONE_APP
        private async void OnBackPressed(object sender, BackPressedEventArgs e)
#endif
        {
            var lastPopupPage = PopupNavigation.PopupStack.LastOrDefault();

            if (lastPopupPage != null)
            {
                e.Handled = true;
                var isPrevent = lastPopupPage.SendBackButtonPressed();

                if (!isPrevent)
                   await PopupNavigation.PopAsync();
            }
        }

        public void AddPopup(PopupPage page)
        {
            var popup = new global::Windows.UI.Xaml.Controls.Primitives.Popup();
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();

	        page.Parent = Application.Current.MainPage;

            renderer.Prepare(popup);
            popup.Child = renderer.ContainerElement;
            popup.IsOpen = true;
            page.ForceLayout();
        }

        public void RemovePopup(PopupPage page)
        {
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
            var popup = renderer.Container;
            //((PopupPageRenderer)popup.Child).Dispose();
            if (popup == null)
                return;

            renderer.Destroy();
            popup.Child = null;
            popup.IsOpen = false;
        }
    }
}
