using Android.App;
using Android.Runtime;

using Microsoft.Maui;
using Microsoft.Maui.Hosting;

using Rg.Plugins.Popup.Services;

using System;

namespace SampleMaui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Essentials.Platform.Init(Current);
        }

        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
        }
    }
}
