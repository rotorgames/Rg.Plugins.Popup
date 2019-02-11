using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Services
{
    internal class PopupNavigationImpl : IPopupNavigation
    {
        private readonly List<PopupPage> _popupStack = new List<PopupPage>();

        private IPopupPlatform PopupPlatform
        {
            get
            {
                var popupPlatform = DependencyService.Get<IPopupPlatform>();

                if(popupPlatform == null)
                    throw new InvalidOperationException("You MUST install Rg.Plugins.Popup to each project and call Rg.Plugins.Popup.Popup.Init(); prior to using it.\nSee more info: " + Config.InitializationDescriptionUrl);

                if(!popupPlatform.IsInitialized)
                    throw new InvalidOperationException("You MUST call Rg.Plugins.Popup.Popup.Init(); prior to using it.\nSee more info: " + Config.InitializationDescriptionUrl);

                return popupPlatform;
            }
        }

        public IReadOnlyList<PopupPage> PopupStack => _popupStack;

        public PopupNavigationImpl()
        {
            PopupPlatform.OnInitialized += OnInitialized;
        }

        private async void OnInitialized(object sender, EventArgs e)
        {
            if (PopupStack.Any())
                await PopAllAsync(false);
        }

        public Task PushAsync(PopupPage page, bool animate = true)
        {
            return InvokeThreadSafe(async () =>
            {
                if (page.IsBeingAppeared)
                    return;

                page.IsBeingAppeared = true;

                animate = CanBeAnimated(animate);

                if (animate)
                {
                    page.PreparingAnimation();
                    await AddAsync(page);
                    await page.AppearingAnimation();
                }
                else
                {
                    await AddAsync(page);
                }

                page.IsBeingAppeared = false;
            });
        }

        public Task PopAsync(bool animate = true)
        {
            return InvokeThreadSafe(async () =>
            {
                animate = CanBeAnimated(animate);

                if (!PopupStack.Any())
                    throw new IndexOutOfRangeException("There is not page in PopupStack");

                await RemovePageAsync(PopupStack.Last(), animate);
            });
        }

        public Task PopAllAsync(bool animate = true)
        {
            return InvokeThreadSafe(async () =>
            {
                animate = CanBeAnimated(animate);

                if (!PopupStack.Any())
                    throw new IndexOutOfRangeException("There is not page in PopupStack");

                var popupTasks = PopupStack.ToList().Select(page => RemovePageAsync(page, animate));

                await Task.WhenAll(popupTasks);
            });
        }

        public Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            return InvokeThreadSafe(async () =>
            {
                if (page == null)
                    throw new NullReferenceException("Page can not be null");

                if (page.IsBeingDismissed)
                    return;

                animate = CanBeAnimated(animate);

                page.IsBeingDismissed = true;

                if (animate)
                    await page.DisappearingAnimation();

                await RemoveAsync(page);

                if (animate)
                    page.DisposingAnimation();

                page.IsBeingDismissed = false;
            });
        }

        // Private

        private async Task AddAsync(PopupPage page)
        {
            _popupStack.Add(page);
            await PopupPlatform.AddAsync(page);
        }

        private async Task RemoveAsync(PopupPage page)
        {
            await PopupPlatform.RemoveAsync(page);
            _popupStack.Remove(page);
        }

        // Internal 

        internal void RemovePopupFromStack(PopupPage page)
        {
            if (_popupStack.Contains(page))
                _popupStack.Remove(page);
        }

        #region Animation

        private bool CanBeAnimated(bool animate)
        {
            return animate && PopupPlatform.IsSystemAnimationEnabled;
        }

        #endregion

        #region Helpers

        Task InvokeThreadSafe(Func<Task> action)
        {
            var tcs = new TaskCompletionSource<bool>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await action.Invoke();
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        #endregion
    }
}
