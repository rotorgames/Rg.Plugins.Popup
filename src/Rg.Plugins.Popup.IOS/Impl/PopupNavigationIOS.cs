using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFoundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.IOS.Extensions;
using Rg.Plugins.Popup.IOS.Impl;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(PopupNavigationIOS))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    [Preserve(AllMembers = true)]
    class PopupNavigationIOS : IPopupNavigation
    {
        public void AddPopup(PopupPage page)
        {
            var renderer = page.GetOrCreateRenderer();
            var currentRenderer = GetCurrentPage().GetOrCreateRenderer();

            DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, async () =>
            {
                await currentRenderer.ViewController.PresentViewControllerAsync(renderer.ViewController, false);
            });
        }

        public void RemovePopup(PopupPage page)
        {
            var viewController = page.GetOrCreateRenderer().ViewController;

            if (viewController != null && !viewController.IsBeingDismissed)
            {
                DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, async () => {
                    await viewController.DismissViewControllerAsync(false);
                });
            }

        }

        private Page GetCurrentPage()
        {
            Page lastPage = PopupNavigation.PopupStack.LastOrDefault();
            if (lastPage == null)
            {
                lastPage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();
            }
            if (lastPage == null)
            {
                lastPage = Application.Current.MainPage;
            }
            return lastPage;
        }
    }
}
