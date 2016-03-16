using System.Linq;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Size = Xamarin.Forms.Size;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.IOS.Renderers
{
    public class PopupPageRenderer : PageRenderer
    {
        private PopupPage _element
        {
            get { return (PopupPage) Element; }
        }
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
                ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

                View.AddGestureRecognizer(new UITapGestureRecognizer(OnTap));
            }
        }

        private void OnTap(UITapGestureRecognizer e)
        {
            var view = e.View;
            var location = e.LocationInView(view);
            var subview = view.HitTest(location, null);
            if (subview == view)
            {
                _element.SendBackgroundClick();
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillChangeFrameNotification, KeyBoardUpNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification,KeyBoardDownNotification);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NSNotificationCenter.DefaultCenter.RemoveObserver(this);

            var isNotRemoved = PopupNavigation.PopupStack.Any(e => e == _element);

            // Close all open pages the Popup, if the main page, on which opened PresentViewControllerAsync, destroyed.
            if (isNotRemoved)
            {
                PopupNavigation.PopAll();
            }
        }

        private void KeyBoardUpNotification(NSNotification notifi)
        {
            var r = UIKeyboard.BoundsFromNotification(notifi);
            var screen = UIScreen.MainScreen.Bounds;

            SetElementSize(new Size(r.Width, screen.Height - r.Height));
        }

        private void KeyBoardDownNotification(NSNotification notifi)
        {
            var r = UIKeyboard.BoundsFromNotification(notifi);
            var screen = UIScreen.MainScreen.Bounds;

            SetElementSize(new Size(r.Width, screen.Height));
        }
    }
}
