using Android.Provider;
using Android.Widget;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Droid.Extensions;
using Rg.Plugins.Popup.Pages;

using System;
using System.Threading.Tasks;

namespace Rg.Plugins.Popup.Droid.Impl
{
    public class PopupPlatformDroid : IPopupPlatform
    {
        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        private FrameLayout DecoreView => (FrameLayout)Microsoft.Maui.Essentials.Platform.CurrentActivity.Window.DecorView;

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => GetIsSystemAnimationEnabled();

        public Task AddAsync(PopupPage page)
        {
            FrameLayout decoreView = DecoreView;

            page.Parent = (Microsoft.Maui.Controls.Element)Microsoft.Maui.MauiApplication.Current.Application.Windows[0].Content;

            Microsoft.Maui.Controls.Compatibility.Platform.Android.IVisualElementRenderer renderer = page.GetOrCreateRenderer();

            decoreView.AddView(renderer.View);

            return PostAsync(renderer.View);
        }

        public Task RemoveAsync(PopupPage page)
        {
            Microsoft.Maui.Controls.Compatibility.Platform.Android.IVisualElementRenderer renderer = page.GetOrCreateRenderer();
            if (renderer != null)
            {
                Microsoft.Maui.Controls.VisualElement element = renderer.Element;

                DecoreView.RemoveView(renderer.View);
                renderer.Dispose();

                if (element != null)
                    element.Parent = null;

                return PostAsync(DecoreView);
            }

            return Task.CompletedTask;
        }

        #region System Animation

        [Obsolete]
        private bool GetIsSystemAnimationEnabled()
        {
            float animationScale;
            var context = Popup.Context;

            if (context == null)
                return false;

            animationScale = Settings.System.GetFloat(
                context.ContentResolver,
                Settings.System.AnimatorDurationScale,
                1);

            return animationScale > 0;
        }

        #endregion

        #region Helpers

        Task<bool> PostAsync(Android.Views.View nativeView)
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
