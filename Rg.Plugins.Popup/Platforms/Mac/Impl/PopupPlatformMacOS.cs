using System;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.MacOS.Extensions;
using Rg.Plugins.Popup.MacOS.Impl;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

[assembly: Dependency(typeof(PopupPlatformMacOS))]
namespace Rg.Plugins.Popup.MacOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformMacOS : IPopupPlatform
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

            page.DescendantRemoved += HandleChildRemoved;

            var renderer = page.GetOrCreateRenderer();

            var forcedMainWindow = NSApplication.SharedApplication.MainWindow ?? Application.Current.MainPage.GetOrCreateRenderer().NativeView.Window;
            forcedMainWindow.ContentView.AddSubview(renderer.NativeView);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(PopupPage page)
        {
            page.DescendantRemoved -= HandleChildRemoved;
            page.DisposeModelAndChildrenRenderers();

            return Task.CompletedTask;
        }

        private static void HandleChildRemoved(object sender, ElementEventArgs e)
        {
            var view = e.Element;
            ((VisualElement)view).DisposeModelAndChildrenRenderers();
        }
    }
}
