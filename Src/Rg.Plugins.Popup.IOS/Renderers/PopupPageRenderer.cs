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
    class PopupPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            this.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
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
