using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Tizen.Impl;
using Rg.Plugins.Popup.Tizen.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

[assembly: Dependency(typeof(PopupPlatformTizen))]
namespace Rg.Plugins.Popup.Tizen.Impl
{
    public class PopupPlatformTizen : IPopupPlatform
    {
        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public Task AddAsync(PopupPage page)
        {
            var renderer = Platform.GetOrCreateRenderer(page);
            if (renderer == null)
                return Task.CompletedTask;
            page.Parent = Application.Current.MainPage;

            (renderer as PopupPageRenderer)?.ShowPopup();
            (page as IPageController)?.SendAppearing();
            return Task.Delay(5);
        }

        public Task RemoveAsync(PopupPage page)
        {
            var renderer = Platform.GetRenderer(page);
            if (renderer == null)
                return Task.CompletedTask;

            (page as IPageController)?.SendDisappearing();
            (renderer as PopupPageRenderer)?.ClosePopup();
            page.Parent = null;
            renderer.Dispose();
            return Task.Delay(5);
        }
    }
}
