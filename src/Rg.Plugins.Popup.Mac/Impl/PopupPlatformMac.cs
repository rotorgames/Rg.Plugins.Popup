using AppKit;
using Foundation;

using System;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Mac.Extensions;
using Rg.Plugins.Popup.Mac.Impl;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;

[assembly: Dependency(typeof(PopupPlatformMac))]
namespace Rg.Plugins.Popup.Mac.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformMac : IPopupPlatform
    {
        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            page.DescendantRemoved += HandleChildRemoved;

            var renderer = page.GetOrCreateRenderer();

            NSApplication.SharedApplication.MainWindow.ContentView.AddSubview(renderer.NativeView);
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