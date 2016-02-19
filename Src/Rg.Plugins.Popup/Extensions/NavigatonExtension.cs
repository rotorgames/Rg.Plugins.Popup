using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Extensions
{
    public static class NavigatonExtension
    {
        public static async Task PushPopupAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
            await PopupNavigation.PushAsync(page, animate);
        }

        public static async Task PopPopupAsunc(this INavigation sender, bool animate = true)
        {
            await PopupNavigation.PopAsync(animate);
        }

        public static void PopAllPopupAsunc(this INavigation sender)
        {
            PopupNavigation.PopAllAsync();
        }
    }
}
