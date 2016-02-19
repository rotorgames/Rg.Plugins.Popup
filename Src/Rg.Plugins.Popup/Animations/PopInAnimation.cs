using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class PopInAnimation : FadeBackgroundAnimation
    {
        public double StartScale { get; set; } = 0.8;

        public override void Begin(View content, PopupPage page)
        {
            base.Begin(content, page);
            EasingIn = Easing.CubicOut;
            EasingOut = Easing.CubicIn;
            content.Opacity = 0;
            content.Scale = StartScale;
        }

        public override Task Appearing(View content, PopupPage page)
        {
            base.Appearing(content, page);
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            content.FadeTo(1);
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, StartScale, 1, 
            easing: EasingIn,
            length: Time, 
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            base.Disappearing(content, page);
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            content.FadeTo(0);
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, 1, StartScale, 
            easing: EasingOut,
            length: Time, 
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }
    }
}
