using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Demo.Animations
{
    class UserAnimation : IPopupAnimation
    {
        public void Begin(View content, PopupPage page)
        {
            content.Opacity = 0;
        }

        public async Task Appearing(View content, PopupPage page)
        {
            var topOffset = GetTopOffset(content, page);
            content.TranslationY = topOffset;
            content.Opacity = 1;

            await content.TranslateTo(0, 0, easing: Easing.CubicOut);
        }

        public async Task Disappearing(View content, PopupPage page)
        {
            var topOffset = GetTopOffset(content, page);

            await content.TranslateTo(0, topOffset, easing: Easing.CubicIn);
        }

        private int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }
    }
}
