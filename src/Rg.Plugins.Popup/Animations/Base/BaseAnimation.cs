using System.Threading.Tasks;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Base
{
    internal abstract class BaseAnimation : IPopupAnimation
    {
        public uint Duration { get; set; } = 200;
        public Easing EasingIn { get; protected set; } = Easing.Linear;
        public Easing EasingOut { get; protected set; } = Easing.Linear;

        public abstract void Preparing(View content, PopupPage page);

        public abstract void Disposing(View content, PopupPage page);

        public abstract Task Appearing(View content, PopupPage page);

        public abstract Task Disappearing(View content, PopupPage page);

        protected int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }

        protected int GetLeftOffset(View content, Page page)
        {
            return (int)(content.Width + page.Width) / 2;
        }
    }
}
