using System;
using System.Threading.Tasks;
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

        protected override Task OnAppearingAnimationEnd()
        {
            return Task.Delay(5000);
        }

        protected override Task OnDisappearingAnimationBegin()
        {
            return Task.Delay(2000);
        }
    }
}
