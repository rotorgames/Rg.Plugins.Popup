using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Demo.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnOpenPupup(object sender, EventArgs e)
        {
            var page = new FirstPopupPage();
            Navigation.PushPopupAsync(page);
        }
    }
}
