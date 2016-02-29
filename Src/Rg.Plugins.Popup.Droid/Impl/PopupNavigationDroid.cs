using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Helpers;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(PopupNavigationDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
    [Preserve(AllMembers = true)]
    class PopupNavigationDroid : IPopupNavigation
    {
        private FrameLayout _decoreView
        {
            get { return (FrameLayout)((Activity) Forms.Context).Window.DecorView; }
        }
        public void AddPopup(PopupPage page)
        {
            var decoreView = _decoreView;

            var renderer = GetRenderer(page);

            page.Layout(DependencyService.Get<IScreenHelper>().ScreenSize);

            PlatformHelper.PushPage(page);
            decoreView.AddView(renderer.ViewGroup);
        }

        public void RemovePopup(PopupPage page)
        {
            PlatformHelper.RemovePage(page);
            _decoreView.RemoveView(GetRenderer(page).ViewGroup);
        }

        private IVisualElementRenderer GetRenderer(PopupPage page)
        {
            IVisualElementRenderer renderer = PlatformHelper.GetRenderer(page);
            if (renderer == null)
            {
                renderer = PlatformHelper.CreateRenderer(page);
                PlatformHelper.SetRenderer(page, renderer);
            }
            return renderer;
        }
    }
}