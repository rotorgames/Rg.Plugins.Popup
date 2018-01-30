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

        public async Task PushAsync(PopupPage page, bool animate = true)
        {
            animate = CanBeAnimated(animate);

            if (animate)
            {
                page.PreparingAnimation();
                await AddAsync(page);
                await Task.Delay(10);
                await page.AppearingAnimation();
            }
            else
            {
                await AddAsync(page);
            }
        }

        public Task PopAsync(bool animate = true)
        {
            animate = CanBeAnimated(animate);

            if (PopupStack.Count == 0)
                throw new IndexOutOfRangeException("There is not page in PopupStack");

            return RemovePageAsync(PopupStack.Last(), animate);
        }

        public async Task PopAllAsync(bool animate = true)
        {
            animate = CanBeAnimated(animate);

            var popupTasks = _popupStack.ToList().Select(page => RemovePageAsync(page, animate));

            await Task.WhenAll(popupTasks);
        }

        public async Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            if (page == null)
                throw new NullReferenceException("Page can not be null");

            if(page.IsBeingDismissed)
                return;

            animate = CanBeAnimated(animate);

            page.IsBeingDismissed = true;

            if (animate)
                await page.DisappearingAnimation();

            await RemoveAsync(page);
            await Task.Delay(50);

            if (animate)
                page.DisposingAnimation();

            page.IsBeingDismissed = false;

            await Task.Delay(5);
        }

        // Private

        private async Task AddAsync(PopupPage page)
        {
            _popupStack.Add(page);
            await PopupPlatform.AddAsync(page);
        }

        private async Task RemoveAsync(PopupPage page)
        {
            _popupStack.Remove(page);
            await PopupPlatform.RemoveAsync(page);
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
    }
}
