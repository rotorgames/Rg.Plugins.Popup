using System.Linq;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Windows.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#elif WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
using Windows.Phone.UI.Input;
#endif
using Page = Xamarin.Forms.Page;

[assembly:ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Windows.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private PopupPage _element
        {
            get { return (PopupPage) Element; }
        }

        [Preserve]
        public PopupPageRenderer()
        {
            
        }

        private void OnKeyboardHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Element?.ForceLayout();
        }

        private void OnKeyboardShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Element?.ForceLayout();
        }

        internal void Prepare()
        {
            Window.Current.SizeChanged += OnSizeChanged;
            DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += OnBackPressed;
#elif WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
#endif

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing += OnKeyboardShowing;
            inputPane.Hiding += OnKeyboardHiding;

            ContainerElement.PointerPressed += OnBackgroundClick;
        }

        internal void Destroy()
        {
            Window.Current.SizeChanged -= OnSizeChanged;
            DisplayInformation.GetForCurrentView().OrientationChanged -= OnOrientationChanged;

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed -= OnBackPressed;
#elif WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
#endif

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing -= OnKeyboardShowing;
            inputPane.Hiding -= OnKeyboardHiding;
        }

        private void OnOrientationChanged(DisplayInformation sender, object args)
        {
            Element?.ForceLayout();
        }

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Element?.ForceLayout();
        }

        private void OnBack()
        {
            if (PopupNavigation.PopupStack.LastOrDefault() == Element)
            {
                var isPrevent = Element.SendBackButtonPressed();
                if (!isPrevent)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.PopAsync();
                    });
                }
            }
        }

        private void OnBackgroundClick(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                _element.SendBackgroundClick();
            }
        }

#if WINDOWS_PHONE_APP
        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            OnBack();
        }
#elif WINDOWS_UWP
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            OnBack();
        }
#endif
    }
}
