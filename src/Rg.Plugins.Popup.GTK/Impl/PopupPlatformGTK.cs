using System;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.GTK.Renderers;
using Rg.Plugins.Popup.GTK.Impl;
using Rg.Plugins.Popup.GTK.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XPlatform = Xamarin.Forms.Platform.GTK.Platform;
using Xamarin.Forms.Platform.GTK.Extensions;
using Xamarin.Forms.Platform.GTK.Controls;

[assembly: Dependency(typeof(PopupPlatformGTK))]
namespace Rg.Plugins.Popup.GTK.Impl
{
    [Preserve(AllMembers = true)]
    class PopupPlatformGTK : IPopupPlatform
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
        public PopupPlatformGTK()
        {

        }



        private async void OnBackPressed(object sender)
        {
            var lastPopupPage = PopupNavigationInstance.PopupStack.LastOrDefault();

            if (lastPopupPage != null)
            {
                var isPrevent = lastPopupPage.DisappearingTransactionTask != null || lastPopupPage.SendBackButtonPressed();

                if (!isPrevent)
                {
                    //e.Handled = true;
                    await PopupNavigationInstance.PopAsync();
                }
            }
        }

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var popup = new PopupControl();
            var app = new base_app(page);

            popup.LoadApplication((Xamarin.Forms.Application)app);
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
            renderer.Prepare(popup);

            popup.IsFloating = true;

            popup.Show();

        }
        public async Task RemoveAsync(PopupPage page)
        {
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
            var popup = renderer.Container;

            if (popup != null)
            {
                renderer.Destroy();
                Cleanup(page);
            }

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
