using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class ListViewPage : PopupPage
    {
        public ListViewPage()
        {
            InitializeComponent();
            listView.ItemsSource = new List<string>
            {
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView"
            };
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
