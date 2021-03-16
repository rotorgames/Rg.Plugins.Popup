using System;
using CoreGraphics;
using Foundation;
using Rg.Plugins.Popup.Pages;
using UIKit;

namespace Rg.Plugins.Popup.IOS.Platform
{
    [Preserve(AllMembers = true)]
    [Register("RgPopupWindow")]
    internal class PopupWindow : UIWindow
    {
        public PopupWindow(IntPtr handle) : base(handle)
        {
            // Fix #307
        }

        public PopupWindow()
        {

        }

        public PopupWindow(UIWindowScene uiWindowScene) : base(uiWindowScene)
        {

        }

        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            var platformRenderer = (PopupPlatformRenderer?)RootViewController;
            var renderer = platformRenderer?.Renderer;
            var hitTestResult = base.HitTest(point, uievent);

            if (!(platformRenderer?.Renderer?.Element is PopupPage formsElement))
                return hitTestResult;

            if (formsElement.InputTransparent)
                return null!;

            if (formsElement.BackgroundInputTransparent && renderer?.NativeView == hitTestResult)
            {
                formsElement.SendBackgroundClick();
                return null!;
            }

            return hitTestResult;
        }
    }
}
