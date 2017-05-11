using Windows.Foundation;
using Windows.UI.ViewManagement;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Thickness = Xamarin.Forms.Thickness;

#if WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI.Xaml;
#endif

[assembly: Dependency(typeof(ScreenHelperWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    [Preserve(AllMembers = true)]
    class ScreenHelperWinPhone : IScreenHelper
    {
        [Preserve]
        public ScreenHelperWinPhone()
        {
        }

        public Rectangle ScreenSize
        {
            get
            {
#if WINDOWS_PHONE_APP || WINDOWS_UWP
                var windowBound = Window.Current.Bounds;
#elif WINDOWS_PHONE
                var windowBound = System.Windows.Application.Current.RootVisual.RenderSize;
#endif
                var inputPane = InputPane.GetForCurrentView();
                var keyboardBounds = inputPane.OccludedRect;

                var bound = new Rect
                {
                    Height = windowBound.Height - keyboardBounds.Height,
                    Width = windowBound.Width,
                };

                return new Rectangle(0, 0, bound.Width, bound.Height);
            }
        }

        public Thickness ScreenOffsets
        {
            get
            {
#if WINDOWS_PHONE_APP || WINDOWS_UWP
                var windowBound = Window.Current.Bounds;
                var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;

                var top = visibleBounds.Top - windowBound.Top;
                var bottom = windowBound.Bottom - visibleBounds.Bottom;
                var left = visibleBounds.Left - windowBound.Left;
                var right = windowBound.Right - visibleBounds.Right;

                return new Thickness(left, top, right, bottom);
#elif WINDOWS_PHONE
                return new Thickness(0, 35, 0, 0);
#endif
            }
        }
    }
}
