using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Contracts
{
    public interface IPopupNavigation
    {
        void AddPopup(PopupPage page);
        void RemovePopup(PopupPage page);
    }
}
