using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Platform.Renderers;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Services
{
    public static class PopupNavigation
    {
        private static readonly List<PopupPage> _popupStack = new List<PopupPage>();

        public static IReadOnlyList<PopupPage> PopupStack
        {
            get { return _popupStack; }
        }

        static PopupNavigation()
        {
            Loader.Load();
        }

        public static Task PushAsync(PopupPage page, bool animate = true)
        {
            var task = new TaskCompletionSource<bool>();
            var parent = GetParentPage();
            page.Parent = parent;
            if (animate)
            {
                page.PreparingAnimation();
                page.ExecuteWhenAppearingOnce(async () =>
                {
                    await page.AppearingAnimation();
                    task.TrySetResult(true);
                });
            }
            DependencyService.Get<IPopupNavigation>().AddPopup(page);
            _popupStack.Add(page);
            if(!animate) task.TrySetResult(true);
            return task.Task;
        }

        public static Task PopAsync(bool animate = true)
        {
            if (PopupStack.Count == 0) 
                throw new IndexOutOfRangeException("There is not page in PopupStack");

            return RemovePageAsync(PopupStack.Last(), animate);
        }

        public async static Task PopAllAsync(bool animate = true)
        {
            var popupTasks = _popupStack.ToList().Select(page => RemovePageAsync(page, animate));

            await Task.WhenAll(popupTasks);
        }

        public async static Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            if(page == null)
                throw new NullReferenceException("Page can not be null");

            if (!page.IsAnimate)
            {
                if (animate) await page.DisappearingAnimation();
                RemovePopup(page);
                await Task.Delay(50);
                page.DisposingAnimation();
            }
        }

        // Private

        private static void RemovePopup(PopupPage page)
        {
            _popupStack.Remove(page);
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IPopupNavigation>().RemovePopup(page);
            });
        }

        private static Page GetParentPage()
        {
            Page lastPage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();

            if (lastPage == null)
                lastPage = Application.Current.MainPage;

            return lastPage;
        }

        private static void BeginInvokeOnMainThreadIfNeed(Action action)
        {
            if (Device.OS != TargetPlatform.iOS)
            {
                Device.BeginInvokeOnMainThread(action);
            }
            else
            {
                action?.Invoke();
            }
        }
    }
}
