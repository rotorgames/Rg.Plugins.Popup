using AppKit;
using Foundation;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Mac.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Mac.Renderers
{
    [Preserve(AllMembers = true)]
    public sealed class PopupPageRenderer : PageRenderer
    {
        private readonly NSGestureRecognizer _tapGestureRecognizer;

        private PopupPage CurrentElement => (PopupPage)Element;

        #region Main Methods

        public PopupPageRenderer()
        {
            _tapGestureRecognizer = new NSClickGestureRecognizer(OnBackgroundTap)
            {
                Delegate = new OnlyMainViewGestureRecognizerDelegate()
            };
            View.AddGestureRecognizer(_tapGestureRecognizer);
            View.AutoresizingMask |= NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                View?.RemoveGestureRecognizer(_tapGestureRecognizer);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Gestures Methods

        private void OnBackgroundTap(NSClickGestureRecognizer e)
        {
            CurrentElement.SendBackgroundClick();
        }

        private class OnlyMainViewGestureRecognizerDelegate : NSGestureRecognizerDelegate
        {
            public override bool ShouldBegin(NSGestureRecognizer gestureRecognizer)
            {
                var view = gestureRecognizer.View;
                var location = gestureRecognizer.LocationInView(view);
                var subview = view.HitTest(location);
                return Equals(subview, view);
            }
        }
       
        #endregion

        #region Layout Methods

        public override void ViewDidLayout()
        {
            base.ViewDidLayout();

            var currentElement = CurrentElement;

            if (View?.Superview?.Frame == null || currentElement == null)
                return;

            var superviewFrame = View.Superview.Frame;
            var applactionFrame = NSScreen.MainScreen.Frame;
            var systemPadding = new Thickness
            {
                Left = applactionFrame.Left,
                Top = applactionFrame.Top,
                Right = applactionFrame.Right - applactionFrame.Width - applactionFrame.Left,
                Bottom = applactionFrame.Bottom - applactionFrame.Height - applactionFrame.Top
            };

            currentElement.BatchBegin();

            currentElement.SetSystemPadding(systemPadding);
            SetElementSize(new Size(superviewFrame.Width, superviewFrame.Height));

            currentElement.BatchCommit();
        }

        #endregion
    }
}