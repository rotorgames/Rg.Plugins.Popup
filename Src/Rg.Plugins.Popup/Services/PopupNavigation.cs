using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                page.Appearing += async (sender, args) =>
                {
                    await page.AppearingAnimation();
                    task.TrySetResult(true);
                };
            }
            DependencyService.Get<IPopupNavigation>().AddPopup(page);
            _popupStack.Add(page);
            if(!animate) task.TrySetResult(true);
            return task.Task;
        }

        public static async Task PopAsync(bool animate = true)
        {
            if (PopupStack.Count == 0) return;
            var page = PopupStack.Last();
            if (animate)
            {
                await page.DisappearingAnimation();
            }
            RemovePopup(page);
            page.DisposingAnimation();
        }

        public static void PopAllAsync()
        {
            foreach (var page in _popupStack)
            {
                DependencyService.Get<IPopupNavigation>().RemovePopup(page);
            }
            _popupStack.Clear();
        }

        // Private
        private static void RemovePopup(PopupPage page)
        {
            _popupStack.Remove(page);
            DependencyService.Get<IPopupNavigation>().RemovePopup(page);
        }
        private static Page GetParentPage()
        {
            //if (PopupStack.Count > 0)
            //{
            //    return PopupStack.Last();
            //}
            //else
            //{
            //    return Application.Current.MainPage;
            //}

            return Application.Current.MainPage;
        }
    }
}
