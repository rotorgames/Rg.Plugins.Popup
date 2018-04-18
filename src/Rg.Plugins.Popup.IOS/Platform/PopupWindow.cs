using System;
using CoreGraphics;
using UIKit;
using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.IOS.Platform
{
    internal class PopupWindow : UIWindow
    {
        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            var platformRenderer = (PopupPlatformRenderer) RootViewController;
            var formsElement = platformRenderer?.Renderer?.Element as PopupPage;
            var renderer = platformRenderer?.Renderer;
            var hitTestResult = base.HitTest(point, uievent);

            if (formsElement != null && formsElement.BackgroundInputTransparent)
            {
                if (renderer.NativeView == hitTestResult)
                    return null;
            }

            return hitTestResult;
        }
    }
}
