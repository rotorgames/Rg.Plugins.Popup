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
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;

#elif WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
using Xamarin.Forms.Platform.WinRT;
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;

#elif WINDOWS_PHONE
using Windows.Phone.UI.Input;
using Xamarin.Forms.Platform.WinPhone;
using WinPopup = System.Windows.Controls.Primitives.Popup;
#endif

[assembly: Dependency(typeof(PopupNavigationWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    [Preserve(AllMembers = true)]
    class PopupNavigationWinPhone : IPopupNavigation
    {
        [Preserve]
        public PopupNavigationWinPhone()
        {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE
            HardwareButtons.BackPressed += OnBackPressed;
#elif WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
#endif
        }

#if WINDOWS_UWP
        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
#elif WINDOWS_PHONE_APP|| WINDOWS_PHONE
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
            page.Parent = Application.Current.MainPage;

            var popup = new WinPopup();
            var renderer = GetRenderer(page);

            renderer.Prepare(popup);
            popup.Child = renderer.ContainerElement;
            popup.IsOpen = true;
            page.ForceLayout();
        }

        public void RemovePopup(PopupPage page)
        {
            var renderer = GetRenderer(page);

            var popup = renderer.Container;
            //((PopupPageRenderer)popup.Child).Dispose();
            if (popup == null)
            {
                renderer.Destroy();
                return;
            }

            renderer.Destroy();
            popup.Child = null;
            popup.IsOpen = false;
        }

        private PopupPageRenderer GetRenderer(PopupPage page)
        {
#if WINDOWS_PHONE_APP || WINDOWS_UWP
            return (PopupPageRenderer)page.GetOrCreateRenderer();
#elif WINDOWS_PHONE
            return (Platform.GetRenderer(page) ?? Platform.CreateRenderer(page)) as PopupPageRenderer;
#endif
        }
    }
}
