using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Windows.Renderers;
using Xamarin.Forms.Internals;
#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#elif WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
using Windows.Phone.UI.Input;
#endif
using Page = Xamarin.Forms.Page;
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;

[assembly:ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Windows.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        internal WinPopup Container { get; private set; }

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

        internal void Prepare(WinPopup container)
        {
            Container = container;

            Window.Current.SizeChanged += OnSizeChanged;
            DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing += OnKeyboardShowing;
            inputPane.Hiding += OnKeyboardHiding;

            ContainerElement.PointerPressed += OnBackgroundClick;
        }

        internal void Destroy()
        {
            Container = null;

            Window.Current.SizeChanged -= OnSizeChanged;
            DisplayInformation.GetForCurrentView().OrientationChanged -= OnOrientationChanged;

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing -= OnKeyboardShowing;
            inputPane.Hiding -= OnKeyboardHiding;

            ContainerElement.PointerPressed -= OnBackgroundClick;
        }

        private void OnOrientationChanged(DisplayInformation sender, object args)
        {
            Element?.ForceLayout();
        }

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Element?.ForceLayout();
        }

        private void OnBackgroundClick(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                _element.SendBackgroundClick();
            }
        }
    }
}
