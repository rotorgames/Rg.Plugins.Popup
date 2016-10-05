using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Thickness = Xamarin.Forms.Thickness;

[assembly:Dependency(typeof(ScreenHelperWinPhone))]
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
                var windowBound = Window.Current.Bounds;
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
                var windowBound = Window.Current.Bounds;
                var visibleBounds =  ApplicationView.GetForCurrentView().VisibleBounds;

                var top = visibleBounds.Top - windowBound.Top;
                var bottom = windowBound.Bottom - visibleBounds.Bottom;
                var left = visibleBounds.Left - windowBound.Left;
                var right = windowBound.Right - visibleBounds.Right;

                return new Thickness(left, top, right, bottom);
            }
        }
    }
}
