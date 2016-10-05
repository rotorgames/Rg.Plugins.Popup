using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

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
