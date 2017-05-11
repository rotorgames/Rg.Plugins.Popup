using Windows.UI.ViewManagement;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Windows.Renderers;
using Xamarin.Forms.Internals;

#if WINDOWS_UWP
using Windows.UI.Core;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Graphics.Display;
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;

#elif WINDOWS_PHONE_APP
using Windows.UI.Core;
using Xamarin.Forms.Platform.WinRT;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Graphics.Display;
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;

#elif WINDOWS_PHONE
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using WinPopup = System.Windows.Controls.Primitives.Popup;
#endif

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Windows.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        internal WinPopup Container { get; private set; }

        private PopupPage _element
        {
            get { return (PopupPage)Element; }
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

#if WINDOWS_PHONE_APP || WINDOWS_UWP
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
#elif WINDOWS_PHONE
        internal void Prepare(WinPopup container)
        {
            Container = container;

            //Window.Current.SizeChanged += OnSizeChanged;
            //DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing += OnKeyboardShowing;
            inputPane.Hiding += OnKeyboardHiding;

            //ContainerElement.PointerPressed += OnBackgroundClick;
        }

        internal void Destroy()
        {
            Container = null;

            //Window.Current.SizeChanged -= OnSizeChanged;
            //DisplayInformation.GetForCurrentView().OrientationChanged -= OnOrientationChanged;

            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing -= OnKeyboardShowing;
            inputPane.Hiding -= OnKeyboardHiding;

            //ContainerElement.PointerPressed -= OnBackgroundClick;
        }
#endif
    }
}
