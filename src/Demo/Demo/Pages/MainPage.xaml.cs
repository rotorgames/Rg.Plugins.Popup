using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class MainPage : ContentPage
    {
        private LoginPopupPage _loginPopup;

        public MainPage()
        {
            InitializeComponent();
            PopupNavigation.Instance.Pushed += PagePushed;
            PopupNavigation.Instance.Popped += PagePopped;
            PopupNavigation.Instance.PoppedAll += AllPagesPopped;
            PopupNavigation.Instance.RemovePageRequested += RemovePageRequested;
            _loginPopup = new LoginPopupPage();
        }

        private async void OnOpenPupup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_loginPopup);
        }

        private async void OnUserAnimationPupup(object sender, EventArgs e)
        {
            var page = new UserAnimationPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenSystemOffsetPage(object sender, EventArgs e)
        {
            var page = new SystemOffsetPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenListViewPage(object sender, EventArgs e)
        {
            var page = new ListViewPage();
            await PopupNavigation.Instance.PushAsync(page);
        }

        private static void RemovePageRequested(object sender, EventArgs e)
        {
            var pageType = sender.GetType();
            Debug.WriteLine($"RemovePageRequested for {pageType.Name}.");
        }

        private static void AllPagesPopped(object sender, EventArgs e)
        {
            Debug.WriteLine("All popup pages popped.");
        }

        private static void PagePopped(object sender, EventArgs e)
        {
            var pageType = sender.GetType();
            Debug.WriteLine($"{pageType.Name} Page Popped.");
        }

        private static void PagePushed(object sender, EventArgs e)
        {
            var pageType = sender.GetType();
            Debug.WriteLine($"{pageType.Name} Page Pushed.");
        }
    }
}
