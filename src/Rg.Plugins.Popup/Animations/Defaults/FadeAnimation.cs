using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Defaults
{
    internal class FadeAnimation : BaseAnimation
    {
        private double _defaultContentOpacity;

        public override void Preparing(View content, PopupPage page)
        {
            _defaultContentOpacity = content.Opacity;
            content.Opacity = 0;
        }

        public override void Disposing(View content, PopupPage page)
        {
            content.Opacity = _defaultContentOpacity;
        }

        public override Task Appearing(View content, PopupPage page)
        {
            return content.FadeTo(1, Duration, EasingIn);
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            _defaultContentOpacity = content.Opacity;
            return content.FadeTo(0, Duration, EasingOut);
        }
    }
}
