using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Rg.Plugins.Popup.GTK;

namespace Demo.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GtkThemes.Init();
            Gtk.Application.Init();            
            Popup.Init();
            Forms.Init(Rg.Plugins.Popup.GTK.Popup.GetExtraAssemblies());
            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);            
            window.Show();
            Gtk.Application.Run();
        }
    }
}
