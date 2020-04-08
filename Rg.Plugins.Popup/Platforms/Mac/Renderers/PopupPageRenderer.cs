using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.MacOS.Extensions;
using Rg.Plugins.Popup.MacOS.Renderers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using Size = Xamarin.Forms.Size;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.MacOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private readonly NSGestureRecognizer _tapGestureRecognizer;

        internal PopupPage CurrentElement => (PopupPage)Element;

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
            this.UpdateSize();
        }

        #endregion
    }
}
