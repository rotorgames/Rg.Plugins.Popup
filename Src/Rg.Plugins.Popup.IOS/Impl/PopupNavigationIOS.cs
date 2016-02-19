using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFoundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.IOS.Helpers;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(PopupNavigationIOS))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    class PopupNavigationIOS : IPopupNavigation
    {
        public void AddPopup(PopupPage page)
        {
            var renderer = GetRenderer(page);
            var currentRenderer = GetRenderer(GetCurrentPage());

            DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, async () =>
            {
                await currentRenderer.ViewController.PresentViewControllerAsync(renderer.ViewController, false);
            });
        }

        public void RemovePopup(PopupPage page)
        {
            var viewController = PlatformHelper.GetRenderer(page).ViewController;

            if (viewController != null && !viewController.IsBeingDismissed)
            {
                DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, async () => {
                    await viewController.DismissViewControllerAsync(false);
                });
            }

        }

        private IVisualElementRenderer GetRenderer(Page page)
        {
            IVisualElementRenderer renderer = PlatformHelper.GetRenderer(page);
            if (renderer == null)
            {
                renderer = PlatformHelper.CreateRenderer(page);
                PlatformHelper.SetRenderer(page, renderer);
            }
            return renderer;
        }

        private Page GetCurrentPage()
        {
            Page lastPage = PopupNavigation.PopupStack.LastOrDefault();
            if (lastPage == null)
            {
                lastPage = Application.Current.MainPage;
            }
            return lastPage;
        }
    }
}
