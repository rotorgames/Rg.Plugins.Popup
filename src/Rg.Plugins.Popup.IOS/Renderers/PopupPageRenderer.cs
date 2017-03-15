using Foundation;
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
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private readonly UIGestureRecognizer _tapGestureRecognizer;
        private NSObject _willChangeFrameNotificationObserver;
        private NSObject _willHideNotificationObserver;

        private PopupPage _element
        {
            get { return (PopupPage) Element; }
        }

        public PopupPageRenderer()
        {
            _tapGestureRecognizer = new UITapGestureRecognizer(OnTap)
            {
                CancelsTouchesInView = false
            };
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
                ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View?.AddGestureRecognizer(_tapGestureRecognizer);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            View?.RemoveGestureRecognizer(_tapGestureRecognizer);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UnregisterAllObservers();

            _willChangeFrameNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyBoardUpNotification);
            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnregisterAllObservers();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if(ParentViewController == null)
                return;

            if (!IsAttachedToCurrentApplication() ||
                (ParentViewController.IsBeingDismissed && ParentViewController.IsViewLoaded))
            {
                PopupNavigation.RemovePopupFromStack(_element);
            }
        }

        private void UnregisterAllObservers()
        {
            if (_willChangeFrameNotificationObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willChangeFrameNotificationObserver);

            if(_willHideNotificationObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willHideNotificationObserver);

            _willChangeFrameNotificationObserver = null;
            _willHideNotificationObserver = null;
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

        private bool IsAttachedToCurrentApplication()
        {
            if (_element == null)
                return false;

            var parent = _element.Parent;

            while (parent != null)
            {
                if (parent == Application.Current)
                    return true;

                parent = parent.Parent;
            }

            return false;
        }
    }
}
