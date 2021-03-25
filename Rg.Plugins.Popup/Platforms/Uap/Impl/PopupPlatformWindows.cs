using System;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Windows.Renderers;
using Rg.Plugins.Popup.Windows.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XPlatform = Xamarin.Forms.Platform.UWP.Platform;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Core;
using Rg.Plugins.Popup.Exceptions;

[assembly: Dependency(typeof(PopupPlatformWindows))]
namespace Rg.Plugins.Popup.Windows.Impl
{
    [Preserve(AllMembers = true)]
    class PopupPlatformWindows : IPopupPlatform
    {
        private IPopupNavigation PopupNavigationInstance => PopupNavigation.Instance;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        [Preserve]
        public PopupPlatformWindows()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var lastPopupPage = PopupNavigationInstance.PopupStack.LastOrDefault();

            if (lastPopupPage != null)
            {
                var isPrevent = lastPopupPage.DisappearingTransactionTask != null || lastPopupPage.SendBackButtonPressed();

                if (!isPrevent)
                {
                    e.Handled = true;
                    await PopupNavigationInstance.PopAsync();
                }
            }
        }

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var popup = new global::Windows.UI.Xaml.Controls.Primitives.Popup();
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();

            renderer.Prepare(popup);
            popup.Child = renderer.ContainerElement;
            popup.IsOpen = true;
            page.ForceLayout();

            await Task.Delay(5);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
            var popup = renderer.Container;

            if (popup != null)
            {
                renderer.Destroy();

                Cleanup(page);
                page.Parent = null;
                popup.Child = null;
                popup.IsOpen = false;
            }

            await Task.Delay(5);
        }

        internal static void Cleanup(VisualElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var elementRenderer = XPlatform.GetRenderer(element);
            foreach (Element descendant in element.Descendants())
            {
                var child = descendant as VisualElement;
                if (child != null)
                {
                    var childRenderer = XPlatform.GetRenderer(child);
                    if (childRenderer != null)
                    {
                        childRenderer.Dispose();
                        XPlatform.SetRenderer(child, null);
                    }
                }
            }
            if (elementRenderer == null)
                return;

            elementRenderer.Dispose();
            XPlatform.SetRenderer(element, null);
        }
    }
}
