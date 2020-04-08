using System;
using CoreGraphics;
using Rg.Plugins.Popup.Pages;
using Foundation;
using AppKit;

namespace Rg.Plugins.Popup.MacOS.Platform
{
    [Preserve(AllMembers = true)]
    [Register("RgPopupWindow")]
    internal class PopupWindow : NSWindow
    {
        public PopupWindow(IntPtr handle):base(handle)
        {
            // Fix #307
        }

        public PopupWindow()
        {

        }

        /*public override NSView HitTest(CGPoint point, NSEvent uievent)
        {
            var platformRenderer = (PopupPlatformRenderer) RootViewController;
            var formsElement = platformRenderer?.Renderer?.Element as PopupPage;
            var renderer = platformRenderer?.Renderer;
            var hitTestResult = base.HitTest(point, uievent);

            if (formsElement == null)
                return hitTestResult;

            if (formsElement.InputTransparent)
                return null;

            if (formsElement.BackgroundInputTransparent && renderer.NativeView == hitTestResult)
            {
                formsElement.SendBackgroundClick();
                return null;
            }

            return hitTestResult;
        }*/
    }
}
