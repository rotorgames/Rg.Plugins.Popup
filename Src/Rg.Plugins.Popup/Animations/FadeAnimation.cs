using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class FadeAnimation : BaseAnimation
    {
        public override void Begin(View content, PopupPage page)
        {
            page.Opacity = 0;
        }

        public override async Task Appearing(View content, PopupPage page)
        {
            await page.FadeTo(1);
        }

        public override async Task Disappearing(View content, PopupPage page)
        {
            await page.FadeTo(0);
        }
    }
}
