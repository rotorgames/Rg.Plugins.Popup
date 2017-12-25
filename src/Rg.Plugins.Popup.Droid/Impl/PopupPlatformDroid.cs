using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using Android.Widget;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Extensions;
using Rg.Plugins.Popup.Droid.Helpers;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XApplication = Xamarin.Forms.Application;

[assembly: Dependency(typeof(PopupPlatformDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
    [Preserve(AllMembers = true)]
    class PopupPlatformDroid : IPopupPlatform
    {
        private IPopupNavigation PopupNavigationInstance => PopupNavigation.Instance;

        private FrameLayout DecoreView => (FrameLayout)((Activity)Popup.Context).Window.DecorView;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public async Task AddAsync(PopupPage page)
        {
            var decoreView = DecoreView;

            page.Parent = XApplication.Current.MainPage;

            var renderer = page.GetOrCreateRenderer();

            decoreView.AddView(renderer.View);
            UpdateListeners(true);

            await Task.Delay(5);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            var renderer = page.GetOrCreateRenderer();
            if (renderer != null)
            {
                var element = renderer.Element;

                DecoreView.RemoveView(renderer.View);
                renderer.Dispose();

                if(element != null)
                    element.Parent = null;

                UpdateListeners(false);
            }

            await Task.Delay(5);
        }

        #region Listeners

        private void UpdateListeners(bool isAdd)
        {
            var isPrevent = PopupNavigationInstance.PopupStack.Count > 0 || isAdd;

            if (Popup.Context is FormsApplicationActivity)
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
            else if (Popup.Context is FormsAppCompatActivity)
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
            if (PopupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = PopupNavigationInstance.PopupStack.Last();

                var isPreventClose = lastPage.IsBeingDismissed || lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigationInstance.PopAsync();
                    });
                }
            }
            return true;
        }

        #endregion
    }
}