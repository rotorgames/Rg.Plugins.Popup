using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.WPF.Extensions;
using Rg.Plugins.Popup.WPF.Impl;
using Rg.Plugins.Popup.WPF.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XPlatform = Xamarin.Forms.Platform.WPF.Platform;

[assembly: Dependency(typeof(PopupPlatformWPF))]
namespace Rg.Plugins.Popup.WPF.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformWPF : IPopupPlatform
    {
        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var popup = new System.Windows.Controls.Primitives.Popup();
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();

            renderer.Prepare(popup);
            popup.Child = renderer.Control;
            popup.IsOpen = true;
            page.ForceLayout();

            return Task.CompletedTask;
        }

        public Task RemoveAsync(PopupPage page)
        {
            var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
            var popup = renderer.Container;

            if (popup != null)
            {
                renderer.Destroy();
                renderer.Dispose();

                Cleanup(page);
                page.Parent = null;
                popup.Child = null;
                popup.IsOpen = false;
            }

            return Task.CompletedTask;
        }

        private static void Cleanup(VisualElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var elementRenderer = XPlatform.GetRenderer(element);
            foreach (var descendant in element.Descendants())
            {
                if (descendant is VisualElement child)
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
