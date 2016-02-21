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
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Droid.Renderers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Droid.Renderers
{
    class PopupPageRenderer : PageRenderer
    {
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Element.ForceLayout();
            base.OnLayout(changed, l, t, r, b);
        }

        protected override void OnAttachedToWindow()
        {
            ContextExtensions.HideKeyboard(Context, ((Activity)Forms.Context).Window.DecorView);
            base.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(0), () =>
            {
                ContextExtensions.HideKeyboard(Context, ((Activity)Forms.Context).Window.DecorView);
                return false;
            });
            base.OnDetachedFromWindow();
        }
    }
}