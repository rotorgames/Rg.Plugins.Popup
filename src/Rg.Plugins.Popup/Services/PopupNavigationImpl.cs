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
        private static readonly List<PopupPage> _popupStack = new List<PopupPage>();

        public IReadOnlyList<PopupPage> PopupStack => _popupStack;

        public Task PushAsync(PopupPage page, bool animate = true)
        {
            var task = new TaskCompletionSource<bool>();
            if (animate)
            {
                page.PreparingAnimation();
                page.ExecuteWhenAppearingOnce(async () =>
                {
                    await page.AppearingAnimation();
                    task.TrySetResult(true);
                });
            }
            DependencyService.Get<IPopupPlatform>().AddPopup(page);
            _popupStack.Add(page);
            if (!animate) task.TrySetResult(true);
            return task.Task;
        }

        public Task PopAsync(bool animate = true)
        {
            if (PopupStack.Count == 0)
                throw new IndexOutOfRangeException("There is not page in PopupStack");

            return RemovePageAsync(PopupStack.Last(), animate);
        }

        public async Task PopAllAsync(bool animate = true)
        {
            var popupTasks = _popupStack.ToList().Select(page => RemovePageAsync(page, animate));

            await Task.WhenAll(popupTasks);
        }

        public async Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            if (page == null)
                throw new NullReferenceException("Page can not be null");

            if (!page.IsAnimate)
            {
                if (animate)
                    await page.DisappearingAnimation();

                RemovePopup(page);
                await Task.Delay(50);

                if(animate)
                    page.DisposingAnimation();
            }
        }

        // Private

        private void RemovePopup(PopupPage page)
        {
            _popupStack.Remove(page);
            DependencyService.Get<IPopupPlatform>().RemovePopup(page);
        }

        // Internal 

        internal void RemovePopupFromStack(PopupPage page)
        {
            if (_popupStack.Contains(page))
                _popupStack.Remove(page);
        }
    }
}
