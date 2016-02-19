using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ScreenHelperDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
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
    }
}