using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Extensions
{
    public static class NavigationExtension
    {
        public static async Task PushPopupAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
            await PopupNavigation.PushAsync(page, animate);
        }

        public static async Task PopPopupAsync(this INavigation sender, bool animate = true)
        {
            await PopupNavigation.PopAsync(animate);
        }

        public static void PopAllPopupAsync(this INavigation sender)
        {
            PopupNavigation.PopAllAsync();
        }
    }
}
