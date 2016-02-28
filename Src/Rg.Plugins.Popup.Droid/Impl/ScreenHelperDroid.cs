using Android.App;
using Android.Graphics;
using Android.Runtime;
using Android.Widget;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ScreenHelperDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
    [Preserve(AllMembers = true)]
    class ScreenHelperDroid : IScreenHelper
    {
        public Rectangle ScreenSize
        {
            get
            {
                var decoreView = (FrameLayout)((Activity)Forms.Context).Window.DecorView;
                return new Rectangle(0.0, 0.0, ContextExtensions.FromPixels(Forms.Context, decoreView.Width),
                    ContextExtensions.FromPixels(Forms.Context, decoreView.Height));
            }
        }

        public Thickness ScreenOffsets
        {
            get
            {
                var decoreView = (FrameLayout) ((Activity) Forms.Context).Window.DecorView;
                Rect visibleRect = new Rect();
                decoreView.GetWindowVisibleDisplayFrame(visibleRect);

                var decoreHeight = decoreView.Height;
                var decoreWidht = decoreView.Width;

                var result = new Thickness
                {
                    Top = FromPixels(visibleRect.Top),
                    Bottom = FromPixels(decoreHeight - visibleRect.Bottom),
                    Right = FromPixels(decoreWidht - visibleRect.Right),
                    Left = FromPixels(visibleRect.Left)
                };

                return result;
            }
        }

        private double FromPixels(int value)
        {
            return ContextExtensions.FromPixels(Forms.Context, value);
        }
    }
}