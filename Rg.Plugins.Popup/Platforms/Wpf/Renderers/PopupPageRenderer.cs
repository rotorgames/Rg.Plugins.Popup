using System;
using System.Windows;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.WPF.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.WPF;
using Application = System.Windows.Application;
using WinPopup = System.Windows.Controls.Primitives.Popup;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.WPF.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        internal WinPopup Container { get; private set; }

        private PopupPage CurrentElement => (PopupPage)Element;

        internal void Prepare(WinPopup container)
        {
            Container = container;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.SizeChanged += OnSizeChanged;

            UpdateElementSize();
            CurrentElement.CloseWhenBackgroundIsClicked = true;
            Container.AllowsTransparency = true;
            Container.MouseDown += Container_MouseDown;
        }

        private void Container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Container.IsOpen = false;
        }

        internal void Destroy()
        {
            Container.MouseDown -= Container_MouseDown;
            Container = null;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateElementSize();
        }

        private void UpdateElementSize()
        {
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
            var rectangle = new Rectangle(windowBound.X, windowBound.Y, windowBound.Width, windowBound.Height);
            CurrentElement.Layout(rectangle);

            CurrentElement.BatchCommit();
            Container.VerticalOffset = rectangle.X;
            Container.HorizontalOffset = rectangle.Y;
        }
    }
}
