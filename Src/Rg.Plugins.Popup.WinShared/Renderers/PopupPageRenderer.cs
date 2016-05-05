using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.WinPhone.Renderers;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#elif WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
using Windows.Phone.UI.Input;
#endif
using Page = Xamarin.Forms.Page;

[assembly:ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.WinPhone.Renderers
{
    public class PopupPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
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
            }
            else if(e.OldElement != null)
            {
                OnDispose();
            }
        }

        private void OnKeyboardHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Element?.ForceLayout();
        }

        private void OnKeyboardShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Element?.ForceLayout();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnDispose();
        }

        private void OnDispose()
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
                if (!isPrevent) Task.Run(() => PopupNavigation.PopAsync());
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

        internal void Destroy()
        {
            Dispose(true);
        }
    }
}
