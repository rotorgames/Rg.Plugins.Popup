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

        private static void AllPagesPopped(object sender, AllPagesPoppedEventArgs e)
        {
            Debug.WriteLine($"Popped {e.PoppedPages.Count()} page(s).");
        }

        private static void PagePopped(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine($"{e.Page.GetType().Name} page popped.");
        }

        private static void PagePushed(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine($"{e.Page.GetType().Name} page pushed.");
        }
    }
}
