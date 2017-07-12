using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using System;

namespace Rg.Plugins.Popup.Extensions
{
    public static class NavigationExtension
    {
        public static Task PushPopupAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
			if (page.Parent != null)
			{
				throw new ArgumentException("Page must not already have a parent.");
			}
            return PopupNavigation.PushAsync(page, animate);
        }

        public static Task PopPopupAsync(this INavigation sender, bool animate = true)
        {
            return PopupNavigation.PopAsync(animate);
        }

        public static Task PopAllPopupAsync(this INavigation sender, bool animate = true)
        {
            return PopupNavigation.PopAllAsync(animate);
        }

        public static Task RemovePopupPageAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
            return PopupNavigation.RemovePageAsync(page, animate);
        }
    }
}
