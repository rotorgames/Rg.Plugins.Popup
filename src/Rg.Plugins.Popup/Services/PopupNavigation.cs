using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
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
            BeginInvokeOnMainThreadIfNeed(() =>
            {
                DependencyService.Get<IPopupNavigation>().AddPopup(page);
            });
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

        public static Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            if(page == null)
                throw new NullReferenceException("Page can not be null");

            var task = new TaskCompletionSource<bool>();
            if (!page.IsAnimate)
            {
                BeginInvokeOnMainThreadIfNeed(async () =>
                {
                    if (animate) await page.DisappearingAnimation();
                    RemovePopup(page);
                    page.DisposingAnimation();
                    task.TrySetResult(true);
                });
            }
            else
            {
                task.TrySetResult(true);
            }

            return task.Task;
        }

        // Internals 

        internal static void RemoveFromStack(PopupPage page)
        {
            if (_popupStack.Any(popupPage => popupPage == page))
            {
                _popupStack.Remove(page);
            }
            else
            {
                throw new ArgumentOutOfRangeException(page.GetType().Name, "There is not page in PopupStack");
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
