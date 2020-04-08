using UIKit;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System;

namespace Rg.Plugins.Popup.IOS.Platform
{
    [Preserve(AllMembers = true)]
    [Register("RgPopupPlatformRenderer")]
    internal class PopupPlatformRenderer : UIViewController
    {
        private IVisualElementRenderer _renderer;

        public IVisualElementRenderer Renderer => _renderer;

        public PopupPlatformRenderer(IVisualElementRenderer renderer)
        {
            _renderer = renderer;
        }

        public PopupPlatformRenderer(IntPtr handle) : base(handle)
        {
            // Fix #307
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _renderer = null;
            }

            base.Dispose(disposing);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].GetSupportedInterfaceOrientations();
            }
            return base.GetSupportedInterfaceOrientations();
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].PreferredInterfaceOrientationForPresentation();
            }
            return base.PreferredInterfaceOrientationForPresentation();
        }

        public override UIViewController ChildViewControllerForStatusBarHidden()
        {
            return _renderer.ViewController;
        }

        public override bool PrefersStatusBarHidden()
        {
            return _renderer.ViewController.PrefersStatusBarHidden();
        }

        public override UIViewController ChildViewControllerForStatusBarStyle()
        {
            return _renderer.ViewController;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return _renderer.ViewController.PreferredStatusBarStyle();
        }

        public override bool ShouldAutorotate()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].ShouldAutorotate();
            }
            return base.ShouldAutorotate();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
            }
            return base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
        }

        public override bool ShouldAutomaticallyForwardRotationMethods => true;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            
            PresentedViewController?.ViewDidLayoutSubviews();
        }
    }
}
