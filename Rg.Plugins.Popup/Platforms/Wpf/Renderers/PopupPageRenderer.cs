using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.WPF.Renderers;
using System;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.WPF;
using WinPopup = System.Windows.Controls.Primitives.Popup;
using Application = System.Windows.Application;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.WPF.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        internal WinPopup Container { get; private set; }

        private PopupPage CurrentElement => (PopupPage)Element;

        [Preserve]
        public PopupPageRenderer()
        {

        }
        
        internal void Prepare(WinPopup container)
        {
            Container = container;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.SizeChanged += OnSizeChanged;

            CurrentElement.CloseWhenBackgroundIsClicked = true;
        }

        internal void Destroy()
        {
            Container = null;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.SizeChanged -= OnSizeChanged;
        }
        
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateElementSize();
        }

        private async void UpdateElementSize()
        {
            await Task.Delay(50);

            var windowBound = Application.Current.MainWindow.RestoreBounds;
            var visibleBounds = Application.Current.MainWindow.RestoreBounds;

            var top = visibleBounds.Top - windowBound.Top;
            var bottom = windowBound.Bottom - visibleBounds.Bottom;
            var left = visibleBounds.Left - windowBound.Left;
            var right = windowBound.Right - visibleBounds.Right;

            top = Math.Max(0, top);
            bottom = Math.Max(0, bottom);
            left = Math.Max(0, left);
            right = Math.Max(0, right);

            var systemPadding = new Xamarin.Forms.Thickness(left, top, right, bottom);

            CurrentElement.BatchBegin();

            CurrentElement.Padding = systemPadding;
            CurrentElement.Layout(new Rectangle(windowBound.X, windowBound.Y, windowBound.Width, windowBound.Height));

            CurrentElement.BatchCommit();
        }
    }
}
