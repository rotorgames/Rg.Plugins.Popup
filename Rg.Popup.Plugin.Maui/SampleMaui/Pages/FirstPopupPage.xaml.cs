using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class FirstPopupPage : PopupPage
    {
        public FirstPopupPage()
        {
            InitializeComponent();
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return Content.FadeTo(0.5);
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return Content.FadeTo(1);
        }
    }
}
