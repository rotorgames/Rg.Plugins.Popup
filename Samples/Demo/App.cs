using Demo.Pages;
using Xamarin.Forms;

namespace Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // The root page of your application
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#7dbbe6"),
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
