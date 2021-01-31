using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Demo.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public AppDelegate()
        {
            const NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable;

            var rect = new CoreGraphics.CGRect(200, 200, 1024, 768);
            MainWindow = new NSWindow(rect, style, NSBackingStore.Buffered, false)
            {
                Title = "Demo",
                TitleVisibility = NSWindowTitleVisibility.Hidden
            };
            MainWindow.Center();
            NSApplication.SharedApplication.MainMenu = new NSMenu();
        }

        public sealed override NSWindow MainWindow { get; }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Rg.Plugins.Popup.Popup.Init();
            Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }
    }
}
