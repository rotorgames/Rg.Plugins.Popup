using System;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class UserAnimationFromStylePage
    {
        public UserAnimationFromStylePage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
