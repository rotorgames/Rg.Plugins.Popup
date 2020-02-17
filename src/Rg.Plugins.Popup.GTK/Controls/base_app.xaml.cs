using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.GTK.Controls
{
    public partial class base_app : Application
    {
        public base_app(Pages.PopupPage page)
        {
            InitializeComponent();

            MainPage = page;
            MainPage.BackgroundColor = Color.Transparent;

        }
    }
}