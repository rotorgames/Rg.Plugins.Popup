using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace Demo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.Init();
            Rg.Plugins.Popup.Popup.Init();
            LoadApplication(new Demo.App());
        }
    }
}
