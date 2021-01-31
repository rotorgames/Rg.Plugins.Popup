using System;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class UserAnimationFromResourcePage
    {
        public UserAnimationFromResourcePage()
        {
            InitializeComponent();
        }


        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
