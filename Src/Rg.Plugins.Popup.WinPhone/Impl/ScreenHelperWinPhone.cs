using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.WinPhone.Impl;
using Xamarin.Forms;
using Thickness = Xamarin.Forms.Thickness;

[assembly:Dependency(typeof(ScreenHelperWinPhone))]
namespace Rg.Plugins.Popup.WinPhone.Impl
{
    class ScreenHelperWinPhone : IScreenHelper
    {
        public Rectangle ScreenSize
        {
            get
            {
                return new Rectangle(0, 0, Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        public Thickness ScreenOffsets
        {
            get
            {
                var visibleBounds =  ApplicationView.GetForCurrentView().VisibleBounds;
                var inputPane = InputPane.GetForCurrentView();
                var keyboardBounds = inputPane.OccludedRect;

                var bottom = visibleBounds.Bottom - visibleBounds.Height - visibleBounds.Top + keyboardBounds.Height;

                return new Thickness(visibleBounds.Left, visibleBounds.Top, ScreenSize.Width - visibleBounds.Right, bottom);
            }
        }
    }
}
