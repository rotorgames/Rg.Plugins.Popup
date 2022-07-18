
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
            if (page.AndroidTalkbackAccessibilityWorkaround)
            {
                HandleAccessibility(true);
                DisableFocusableInTouchMode(XApplication.Current.MainPage?.GetOrCreateRenderer().View.Parent);
            }
            
            page.Parent = XApplication.Current.MainPage;
            var renderer = page.GetOrCreateRenderer();

            DecoreView?.AddView(renderer.View);
            return PostAsync(renderer.View);

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
                if (page.AndroidTalkbackAccessibilityWorkaround)
                {
                    HandleAccessibility(false);
                }

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
        #endregion

        static void HandleAccessibility(bool showPopup)
        {            
            Page? mainPage = XApplication.Current?.MainPage;

            if (mainPage is null)
            {
                return;
            }

            int navCount = mainPage.Navigation.NavigationStack.Count;
            int modalCount = mainPage.Navigation.ModalStack.Count;

            ProcessView(showPopup, mainPage.GetOrCreateRenderer().View);

            if (navCount > 0)
            {
                ProcessView(showPopup, mainPage.Navigation?.NavigationStack[navCount - 1]?.GetOrCreateRenderer().View);
            }

            if (modalCount > 0)
            {
                ProcessView(showPopup, mainPage.Navigation?.ModalStack[modalCount - 1]?.GetOrCreateRenderer()?.View);
            }

            static void ProcessView(bool showPopup, Android.Views.View? view)
            {
                if (view is null)
                {
                    return;
                }

                // Screen reader
                view.ImportantForAccessibility = showPopup ? ImportantForAccessibility.NoHideDescendants : ImportantForAccessibility.Auto;

                // Keyboard navigation
                ((ViewGroup)view).DescendantFocusability = showPopup ? DescendantFocusability.BlockDescendants : DescendantFocusability.AfterDescendants;
                view.ClearFocus();
            }
        }
    }
}
