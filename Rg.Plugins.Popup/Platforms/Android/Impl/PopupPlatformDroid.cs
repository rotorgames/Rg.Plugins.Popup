using System;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Extensions;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Exceptions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

using Xamarin.Forms;

using XApplication = Xamarin.Forms.Application;

[assembly: Dependency(typeof(PopupPlatformDroid))]
namespace Rg.Plugins.Popup.Droid.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformDroid : IPopupPlatform
    {
        private static FrameLayout? DecoreView => (FrameLayout?)((Activity?)Popup.Context)?.Window?.DecorView;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => GetIsSystemAnimationEnabled();

        public Task AddAsync(PopupPage page)
        {
            var decoreView = DecoreView;

            RecursivelyChangeAccessibilityOfViewChildren(XApplication.Current.MainPage.GetOrCreateRenderer().View, ImportantForAccessibility.No);

            page.Parent = XApplication.Current.MainPage;

            var renderer = page.GetOrCreateRenderer();
            decoreView?.AddView(renderer.View);

            return PostAsync(renderer.View);
        }

        public Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var renderer = page.GetOrCreateRenderer();
            if (renderer != null)
            {
                RecursivelyChangeAccessibilityOfViewChildren(XApplication.Current.MainPage.GetOrCreateRenderer().View, ImportantForAccessibility.Auto);

                page.Parent = XApplication.Current.MainPage;
                var element = renderer.Element;

                DecoreView?.RemoveView(renderer.View);
                renderer.Dispose();

                if (element != null)
                    element.Parent = null;
                if (DecoreView != null)
                    return PostAsync(DecoreView);
            }

            return Task.FromResult(true);
        }

        #region System Animation

        private static bool GetIsSystemAnimationEnabled()
        {
            float animationScale;
            var context = Popup.Context;

            if (context == null)
                return false;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
            {
                animationScale = Settings.Global.GetFloat(
                    context.ContentResolver,
                    Settings.Global.AnimatorDurationScale,
                    1);
            }
            else
            {
                animationScale = Settings.System.GetFloat(
                    context.ContentResolver,
                    Settings.System.AnimatorDurationScale,
                    1);
            }

            return animationScale > 0;
        }

        #endregion

        #region Helpers

        private static Task PostAsync(Android.Views.View nativeView)
        {
            if (nativeView == null)
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();

            nativeView.Post(() => tcs.SetResult(true));

            return tcs.Task;
        }

        private void RecursivelyChangeAccessibilityOfViewChildren(Android.Views.View view, ImportantForAccessibility important)
        {
            if (view is ViewGroup vGroup)
            {
                for (int i = 0; i < vGroup.ChildCount; i++)
                {
                    Android.Views.View vChild = vGroup.GetChildAt(i);
                    vChild.ImportantForAccessibility = important;
                    vChild.ClearFocus();
                    RecursivelyChangeAccessibilityOfViewChildren(vChild, important);
                }
            }
        }
        #endregion
    }
}
