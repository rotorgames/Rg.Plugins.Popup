using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class FadeBackgroundAnimation : BaseAnimation
    {
        private Color _backgroundColor;

        public uint BackgroundTime { get; protected set; }
        public Easing BackgroundEasingIn { get; protected set; } = Easing.Linear;
        public Easing BackgroundEasingOut { get; protected set; } = Easing.Linear;

        public FadeBackgroundAnimation()
        {
            BackgroundTime = Time;
        }
        public override void Preparing(View content, PopupPage page)
        {
            if (page.IsBackgroundAnimating)
            {
                _backgroundColor = page.BackgroundColor;
                page.BackgroundColor = GetColor(0);
            }
        }

        public override Task Appearing(View content, PopupPage page)
        {
            if (!page.IsBackgroundAnimating) return null;
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            page.Animate("backgroundFade", d =>
            {
                page.BackgroundColor = GetColor(d);
            }, 0, _backgroundColor.A, length: BackgroundTime, finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            if (!page.IsBackgroundAnimating) return null;
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            page.Animate("backgroundFade", d =>
            {
                page.BackgroundColor = GetColor(d);
            }, _backgroundColor.A, 0, length: BackgroundTime, finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        private Color GetColor(double transparent)
        {
            return new Color(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, transparent);
        }
    }
}
