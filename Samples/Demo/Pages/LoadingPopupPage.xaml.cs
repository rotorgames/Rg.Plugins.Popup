using Rg.Plugins.Popup.Pages;

namespace Demo.Pages
{
    public partial class LoadingPopupPage : PopupPage
    {
        public LoadingPopupPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
