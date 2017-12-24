using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Demo.Pages
{
    public partial class ReproPage : PopupPage
    {
        public ReproPage()
        {
            InitializeComponent();
        }

        private async void OnOKTapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

		private async void OnCancelTapped(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}
	}
}
