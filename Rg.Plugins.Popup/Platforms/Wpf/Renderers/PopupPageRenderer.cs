using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
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
        internal WinPopup? Container { get; private set; }

        private PopupPage CurrentElement => (PopupPage)Element;

        internal void Prepare(WinPopup container)
        {
            Container = container;
            Container.Placement = PlacementMode.Absolute;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.SizeChanged += OnSizeChanged;
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Activated += OnActivated;

            UpdateElementSize();
            Container.AllowsTransparency = true;
            Container.MouseDown += Container_MouseDown;
            Container.Opened += Container_Opened;
        }

        private void OnActivated(object sender, EventArgs e)
        {
            UpdateZOrder();
        }

        private void Container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CurrentElement.SendBackgroundClick();
        }

        private void Container_Opened(object sender, EventArgs e)
        {
            UpdateZOrder();
        }

        internal void Destroy()
        {
            if (Container != null)
            {
                Container.MouseDown -= Container_MouseDown;
                Container.Opened -= Container_Opened;
                Container = null;
            }

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.SizeChanged -= OnSizeChanged;
                Application.Current.MainWindow.Activated -= OnActivated;
            }
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
            if (Container != null)
            {
                Container.VerticalOffset = rectangle.Y;
                Container.HorizontalOffset = rectangle.X;
            }
        }

        private void UpdateZOrder()
        {
            if (Container != null)
                _ = SetWindowPos(GetHwnd(Container.Child), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }

        private static IntPtr GetHwnd(Visual visual)
        {
            var hwndSource = ((HwndSource)PresentationSource.FromVisual(visual));
            if(hwndSource==null)
            {
                return IntPtr.Zero;
            }
            return hwndSource.Handle;
        }

        private const int HWND_NOTOPMOST = -2;
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOSIZE = 1;

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
    }
}
