using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Droid.Renderers;
using Rg.Plugins.Popup.Pages;

namespace SampleMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureLifecycleEvents(lifecycle =>
                {
#if ANDROID
                    lifecycle.AddAndroid(d => d.OnBackPressed(activity => Popup.SendBackPressed(activity.OnBackPressed)));
#endif
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    //handlers.AddCompatibilityRenderer(typeof(PopupPage), typeof(PopupPageRenderer));
                });

            return builder.Build();
        }
    }
}
