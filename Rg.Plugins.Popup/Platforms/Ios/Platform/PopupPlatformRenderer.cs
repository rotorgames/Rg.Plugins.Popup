#nullable enable

using System;
using Foundation;
using UIKit;

using Xamarin.Forms.Platform.iOS;

namespace Rg.Plugins.Popup.IOS.Platform
{
    [Preserve(AllMembers = true)]
    [Register("RgPopupPlatformRenderer")]
    internal class PopupPlatformRenderer : UIViewController
    {
        public IVisualElementRenderer? Renderer { get; private set; }

        public PopupPlatformRenderer(IVisualElementRenderer renderer)
        {
            Renderer = renderer;
        }

        public PopupPlatformRenderer(IntPtr handle) : base(handle)
        {
            // Fix #307
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Renderer = null;
            }

            base.Dispose(disposing);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
            => Renderer?.ViewController?.GetSupportedInterfaceOrientations() ??
               base.GetSupportedInterfaceOrientations();

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
            => Renderer?.ViewController?.PreferredInterfaceOrientationForPresentation() ??
               base.PreferredInterfaceOrientationForPresentation();

        public override UIViewController? ChildViewControllerForStatusBarHidden()
            => Renderer?.ViewController?.ChildViewControllerForStatusBarHidden() ??
               base.ChildViewControllerForStatusBarHidden();

        public override bool PrefersStatusBarHidden()
            => Renderer?.ViewController?.PrefersStatusBarHidden() ??
               base.PrefersStatusBarHidden();

        public override UIViewController? ChildViewControllerForStatusBarStyle()
            => Renderer?.ViewController ??
               base.ChildViewControllerForStatusBarStyle();

        public override UIStatusBarStyle PreferredStatusBarStyle()
            => Renderer?.ViewController?.PreferredStatusBarStyle() ??
               base.PreferredStatusBarStyle();

        public override bool ShouldAutorotate()
            => Renderer?.ViewController?.ShouldAutorotate() ??
               base.ShouldAutorotate();

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
            => Renderer?.ViewController?.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation) ??
               base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);

        public override bool ShouldAutomaticallyForwardRotationMethods => true;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            PresentedViewController?.ViewDidLayoutSubviews();
        }
    }
}
