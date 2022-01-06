using System.Diagnostics;

using Rg.Plugins.Popup.Services;

namespace SampleMaui.CSharpMarkup
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BuildContent();

            PopupNavigation.Instance.Pushing += (sender, e) => Debug.WriteLine($"[Popup] Pushing: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Pushed += (sender, e) => Debug.WriteLine($"[Popup] Pushed: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Popping += (sender, e) => Debug.WriteLine($"[Popup] Popping: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Popped += (sender, e) => Debug.WriteLine($"[Popup] Popped: {e.Page.GetType().Name}");
        }

        protected override void OnAppearing()
        {

        }
    }
}
