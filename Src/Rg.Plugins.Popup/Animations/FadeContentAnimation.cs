using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    class FadeContentAnimation : FadeBackgroundAnimation
    {
        public override void Begin(View content, PopupPage page)
        {
            base.Begin(content, page);
            content.Opacity = 0;
        }

        public override Task Appearing(View content, PopupPage page)
        {
            base.Appearing(content, page);
            return content.FadeTo(1, Time, EasingIn);
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            base.Disappearing(content, page);
            return content.FadeTo(0, Time, EasingOut);
        }
    }
}
