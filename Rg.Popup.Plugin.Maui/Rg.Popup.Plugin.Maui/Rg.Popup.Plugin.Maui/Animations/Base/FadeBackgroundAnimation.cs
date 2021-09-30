
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using Rg.Plugins.Popup.Pages;

using System.Threading.Tasks;

namespace Rg.Plugins.Popup.Animations.Base
{
    public abstract class FadeBackgroundAnimation : BaseAnimation
    {
        private Color _backgroundColor;

        public bool HasBackgroundAnimation { get; set; } = true;

        public override void Preparing(View content, PopupPage page)
        {
            if (AnimatedAndCompatible(page))
            {
                _backgroundColor = page.BackgroundColor;
                page.BackgroundColor = GetColor(0);
            }
        }

        public override void Disposing(View content, PopupPage page)
        {
            if (AnimatedAndCompatible(page))
            {
                page.BackgroundColor = _backgroundColor;
            }
        }

        public override Task Appearing(View content, PopupPage page)
        {
            if (AnimatedAndCompatible(page))
            {
                TaskCompletionSource<bool> task = new();
                page.Animate("backgroundFade", d =>
                {
                    page.BackgroundColor = GetColor(d);
                }, 0, _backgroundColor.Alpha, length: DurationIn, finished: (d, b) =>
                {
                    task.SetResult(true);
                });

                return task.Task;
            }

            return Task.CompletedTask;
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            if (AnimatedAndCompatible(page))
            {
                TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();

                _backgroundColor = page.BackgroundColor;

                page.Animate("backgroundFade", d =>
                {
                    page.BackgroundColor = GetColor(d);
                }, _backgroundColor.Alpha, 0, length: DurationOut, finished: (d, b) =>
                {
                    task.SetResult(true);
                });

                return task.Task;
            }

            return Task.CompletedTask;
        }

        private Color GetColor(double transparent) => new Color(_backgroundColor.Red, _backgroundColor.Green, _backgroundColor.Blue, (float)transparent);
        private bool AnimatedAndCompatible(PopupPage page) => HasBackgroundAnimation && page.BackgroundImageSource == null;
    }
}
