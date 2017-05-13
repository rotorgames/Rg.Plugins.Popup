using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.Pages;
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
        private CGRect _keyboardBounds;

        private PopupPage CurrentElement => (PopupPage) Element;

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

                CurrentElement.Layout(new Rectangle(0, 0, 320, 560));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                View?.RemoveGestureRecognizer(_tapGestureRecognizer);
            }

            base.Dispose(disposing);
        }

        private void OnTap(UITapGestureRecognizer e)
        {
            var view = e.View;
            var location = e.LocationInView(view);
            var subview = view.HitTest(location, null);
            if (subview == view)
            {
                CurrentElement.SendBackgroundClick();
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

            if(View?.Superview == null)
                return;

            var superviewFrame = View.Superview.Frame;
            var applactionFrame = UIScreen.MainScreen.ApplicationFrame;
            var systemPadding = new Thickness
            {
                Left = applactionFrame.Left,
                Top = applactionFrame.Top,
                Right = applactionFrame.Right - applactionFrame.Width - applactionFrame.Left,
                Bottom = applactionFrame.Bottom - applactionFrame.Height - applactionFrame.Top + _keyboardBounds.Height
            };

            CurrentElement.BatchBegin();

            CurrentElement.SetSystemPadding(systemPadding);
            SetElementSize(new Size(superviewFrame.Width, superviewFrame.Height));

            CurrentElement.BatchCommit();
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
            _keyboardBounds = UIKeyboard.BoundsFromNotification(notifi);

            ViewDidLayoutSubviews();
        }

        private async void KeyBoardDownNotification(NSNotification notifi)
        {
            NSObject duration;
            var canAnimated = notifi.UserInfo.TryGetValue(UIKeyboard.AnimationDurationUserInfoKey, out duration);

            _keyboardBounds = CGRect.Empty;

            if(canAnimated)
            {
                //It is needed that buttons are working when keyboard is opened. See #11
                await Task.Delay(70);

                await UIView.AnimateAsync((double)(NSNumber)duration, ViewDidLayoutSubviews);
            }
            else
            {
                ViewDidLayoutSubviews();
            }
        }
    }
}
