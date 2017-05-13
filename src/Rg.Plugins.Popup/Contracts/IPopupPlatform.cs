using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Contracts
{
    internal interface IPopupPlatform
    {
        void AddPopup(PopupPage page);

        void RemovePopup(PopupPage page);
    }
}
