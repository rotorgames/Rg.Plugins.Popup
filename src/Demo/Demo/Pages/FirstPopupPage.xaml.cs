using System;
using Demo.Animations;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Demo.Pages
{
    public partial class FirstPopupPage : PopupPage
    {
        public FirstPopupPage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}
