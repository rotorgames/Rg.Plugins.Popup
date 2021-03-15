using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Demo.Pages
{
    public partial class TestCurrentViewController : PopupPage
    {
        private int index = 0;

        public TestCurrentViewController(int index)
        {
            InitializeComponent();
            this.index = index;
            indexLable.Text = this.index.ToString();
        }

        private async void OnTestCurrentViewController(object sender, EventArgs e)
        {
            var page = new TestCurrentViewController(index + 1);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnFilePicker(object sender, EventArgs e)
        {
            var file = await Xamarin.Essentials.FilePicker.PickAsync();
        }
    }
}
