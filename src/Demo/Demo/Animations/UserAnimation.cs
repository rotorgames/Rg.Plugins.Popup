using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Demo.Animations
{
    class UserAnimation : IPopupAnimation
    {
        public uint Duration { get; set; } = 200;

        public void Preparing(View content, PopupPage page)
        {
            content.Opacity = 0;
        }

        public void Disposing(View content, PopupPage page)
        {
            
        }

        public async Task Appearing(View content, PopupPage page)
        {
            var topOffset = GetTopOffset(content, page);
            content.TranslationY = topOffset;
            content.Opacity = 1;

            await content.TranslateTo(0, 0, Duration, Easing.CubicOut);
        }

        public async Task Disappearing(View content, PopupPage page)
        {
            var topOffset = GetTopOffset(content, page);

            await content.TranslateTo(0, topOffset, Duration, Easing.CubicIn);
        }

        private int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }
    }
}
