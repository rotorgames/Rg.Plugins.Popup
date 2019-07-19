using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Contracts
{
    public interface IPopupNavigation
    {
        event EventHandler<NavigationEventArgs> Pushed;

        event EventHandler<NavigationEventArgs> Popped;

        event EventHandler<AllPagesPoppedEventArgs> PoppedAll;

        event EventHandler<NavigationEventArgs> RemovePageRequested;

        IReadOnlyList<PopupPage> PopupStack { get; }

        Task PushAsync(PopupPage page, bool animate = true);

        Task PopAsync(bool animate = true);

        Task PopAllAsync(bool animate = true);

        Task RemovePageAsync(PopupPage page, bool animate = true);
    }
}