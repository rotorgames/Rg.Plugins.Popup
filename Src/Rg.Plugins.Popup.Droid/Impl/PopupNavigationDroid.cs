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
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(PopupNavigationDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
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

            decoreView.AddView(renderer.ViewGroup);
            UpdateListeners(true);
        }

        public void RemovePopup(PopupPage page)
        {
            _decoreView.RemoveView(GetRenderer(page).ViewGroup);
            UpdateListeners(false);
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

        #region Listeners

        private void UpdateListeners(bool isAdd)
        {
            var isPrevent = PopupNavigation.PopupStack.Count > 0 || isAdd;

            if (Forms.Context is FormsApplicationActivity)
            {
                var handleBackPressed = (FormsApplicationActivity.BackButtonPressedEventHandler)PlatformHelper.GetHandleBackPressed<FormsApplicationActivity.BackButtonPressedEventHandler>();
                FormsApplicationActivity.BackPressed -= handleBackPressed;
                FormsApplicationActivity.BackPressed -= OnBackPressed;
                if (!isPrevent)
                {
                    FormsApplicationActivity.BackPressed += handleBackPressed;
                }
                else
                {
                    FormsApplicationActivity.BackPressed += OnBackPressed;
                }

            }
            else if (Forms.Context is FormsAppCompatActivity)
            {
                var handleBackPressed = (FormsAppCompatActivity.BackButtonPressedEventHandler)PlatformHelper.GetHandleBackPressed<FormsAppCompatActivity.BackButtonPressedEventHandler>();
                FormsAppCompatActivity.BackPressed -= handleBackPressed;
                FormsAppCompatActivity.BackPressed -= OnBackPressed;
                if (!isPrevent)
                {
                    FormsAppCompatActivity.BackPressed += handleBackPressed;
                }
                else
                {
                    FormsAppCompatActivity.BackPressed += OnBackPressed;
                }
            }
        }

        private bool OnBackPressed(object sender, EventArgs e)
        {
            if (PopupNavigation.PopupStack.Count > 0)
            {
                var isPreventClose = PopupNavigation.PopupStack.Last().SendBackButtonPressed();
                if (!isPreventClose)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.PopAsync();
                    });
                }
            }
            return true;
        }

        #endregion
    }
}