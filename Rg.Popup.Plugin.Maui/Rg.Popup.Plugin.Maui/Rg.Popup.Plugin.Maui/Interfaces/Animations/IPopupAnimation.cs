
using Microsoft.Maui.Controls;

using Rg.Plugins.Popup.Pages;

using System.Threading.Tasks;

namespace Rg.Plugins.Popup.Interfaces.Animations
{
    public interface IPopupAnimation
    {
        void Preparing(View content, PopupPage page);
        void Disposing(View content, PopupPage page);
        Task Appearing(View content, PopupPage page);
        Task Disappearing(View content, PopupPage page);
    }
}
