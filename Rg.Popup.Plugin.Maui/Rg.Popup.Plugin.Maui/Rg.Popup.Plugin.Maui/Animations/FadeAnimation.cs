using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Pages;

namespace Rg.Plugins.Popup.Animations
{
    public class FadeAnimation : BaseAnimation
    {
        private double _defaultOpacity;

        public bool HasBackgroundAnimation { get; set; } = true;

        public override void Preparing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation)
            {
                _defaultOpacity = page.Opacity;
                page.Opacity = 0;
            }
            else if (content != null)
            {
                _defaultOpacity = content.Opacity;
                content.Opacity = 0;
            }
        }

        public override void Disposing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation || content != null)
            {
                page.Opacity = _defaultOpacity;
            }

        }

        public override Task Appearing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation)
            {
                return page.FadeTo(1, DurationIn, EasingIn);
            }
            if (content != null)
            {
                return content.FadeTo(1, DurationIn, EasingIn);
            }
            return Task.CompletedTask;
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            _defaultOpacity = page.Opacity;

            if (HasBackgroundAnimation)
            {
                return page.FadeTo(0, DurationOut, EasingOut);
            }
            if (content != null)
            {
                return content.FadeTo(0, DurationOut, EasingOut);
            }
            return Task.CompletedTask;
        }
    }
}
