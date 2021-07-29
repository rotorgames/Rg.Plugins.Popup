using System;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Extensions;
using Rg.Plugins.Popup.Droid.Impl;
using Rg.Plugins.Popup.Exceptions;
using Rg.Plugins.Popup.Pages;

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

            HandleAccessibilityWorkaround(page);

            page.Parent = XApplication.Current.MainPage;
            var renderer = page.GetOrCreateRenderer();

            decoreView?.AddView(renderer.View);
            return PostAsync(renderer.View);

            static void HandleAccessibilityWorkaround(PopupPage page)
            {
                if (page.AndroidTalkbackAccessibilityWorkaround)
                {
                    var navCount = XApplication.Current.MainPage.Navigation.NavigationStack.Count;
                    var modalCount = XApplication.Current.MainPage.Navigation.ModalStack.Count;
                    XApplication.Current.MainPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;

                    if (navCount > 0)
                    {
                        XApplication.Current.MainPage.Navigation.NavigationStack[navCount - 1].GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
                    }
                    if (modalCount > 0)
                    {
                        XApplication.Current.MainPage.Navigation.ModalStack[modalCount - 1].GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
                    }

                    DisableFocusableInTouchMode(XApplication.Current.MainPage.GetOrCreateRenderer().View.Parent);
                }
            }

            static void DisableFocusableInTouchMode(IViewParent? parent)
            {
                var view = parent;
                string className = $"{view?.GetType().Name}";

                while (!className.Contains("PlatformRenderer") && view != null)
                {
                    view = view.Parent;
                    className = $"{view?.GetType().Name}";
                }

                if (view is Android.Views.View androidView)
                {
                    androidView.Focusable = false;
                    androidView.FocusableInTouchMode = false;
                }
            }
        }

        public Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var renderer = page.GetOrCreateRenderer();
            if (renderer != null)
            {
                HandleAccessibilityWorkaround(page);

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

            static void HandleAccessibilityWorkaround(PopupPage page)
            {
                if (page.AndroidTalkbackAccessibilityWorkaround)
                {
                    var navCount = XApplication.Current.MainPage.Navigation.NavigationStack.Count;
                    var modalCount = XApplication.Current.MainPage.Navigation.ModalStack.Count;
                    XApplication.Current.MainPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.Auto;

                    if (navCount > 0)
                    {
                        XApplication.Current.MainPage.Navigation.NavigationStack[navCount - 1].GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.Auto;
                    }
                    if (modalCount > 0)
                    {
                        XApplication.Current.MainPage.Navigation.ModalStack[modalCount - 1].GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.Auto;
                    }
                }
            }
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
        #endregion
    }
}
