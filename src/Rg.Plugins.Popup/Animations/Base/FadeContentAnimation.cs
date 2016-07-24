using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Base
{
    internal class FadeContentAnimation : BaseAnimation
    {
        public override void Preparing(View content, PopupPage page)
        {
            content.Opacity = 0;
        }

        public override void Disposing(View content, PopupPage page)
        {

        }

        public override Task Appearing(View content, PopupPage page)
        {
            return content.FadeTo(1, Duration, EasingIn);
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            return content.FadeTo(0, Duration, EasingOut);
        }
    }
}
