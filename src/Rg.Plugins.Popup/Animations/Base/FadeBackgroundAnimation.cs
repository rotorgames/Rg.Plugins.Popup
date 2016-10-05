using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Base
{
    public abstract class FadeBackgroundAnimation : BaseAnimation
    {
        private Color _backgroundColor;

        public bool HasBackgroundAnimation { get; set; } = true;

        public override void Preparing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation && page.BackgroundImage == null)
            {
                _backgroundColor = page.BackgroundColor;
                page.BackgroundColor = GetColor(0);
            }
        }

        public override void Disposing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation && page.BackgroundImage == null)
            {
                page.BackgroundColor = _backgroundColor;
            }
        }

        public override Task Appearing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation && page.BackgroundImage == null)
            {
                TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
                page.Animate("backgroundFade", d =>
                {
                    page.BackgroundColor = GetColor(d);
                }, 0, _backgroundColor.A, length: DurationIn, finished: (d, b) =>
                {
                    task.SetResult(true);
                });

                return task.Task;
            }

            return Task.FromResult(0);
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation && page.BackgroundImage == null)
            {
                TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();

                _backgroundColor = page.BackgroundColor;

                page.Animate("backgroundFade", d =>
                {
                    page.BackgroundColor = GetColor(d);
                }, _backgroundColor.A, 0, length: DurationOut, finished: (d, b) =>
                {
                    task.SetResult(true);
                });

                return task.Task;
            }

            return Task.FromResult(0);
        }

        private Color GetColor(double transparent)
        {
            return new Color(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, transparent);
        }
    }
}
