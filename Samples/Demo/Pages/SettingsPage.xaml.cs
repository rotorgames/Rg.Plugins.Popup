using System;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
