using Demo.Pages;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Controls.Xaml;

using Application = Microsoft.Maui.Controls.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Skip)]
namespace SampleMaui
{
    public partial class App : Application
    {
        public App() 
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
