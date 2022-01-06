using Microsoft.Maui.LifecycleEvents;
using Rg.Plugins.Popup;
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
                    //handlers.AddMauiControlsHandlers();
                    handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
                    //handlers.AddCompatibilityRenderer(typeof(PopupPage), typeof(PopupPageHandler));
                });
            return builder.Build();
        }
    }
}
