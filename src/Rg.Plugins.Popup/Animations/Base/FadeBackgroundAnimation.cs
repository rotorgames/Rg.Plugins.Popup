using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Base
{
    internal class FadeBackgroundAnimation : BaseAnimation
    {
        private Color _backgroundColor;

        public Easing BackgroundEasingIn { get; protected set; } = Easing.Linear;
        public Easing BackgroundEasingOut { get; protected set; } = Easing.Linear;

        public override void Preparing(View content, PopupPage page)
        {
            _backgroundColor = page.BackgroundColor;
            page.BackgroundColor = GetColor(0);
        }

        public override void Disposing(View content, PopupPage page)
        {
            page.BackgroundColor = _backgroundColor;
        }

        public override Task Appearing(View content, PopupPage page)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            page.Animate("backgroundFade", d =>
            {
                page.BackgroundColor = GetColor(d);
            }, 0, _backgroundColor.A, length: Duration, finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        public override Task Disappearing(View content, PopupPage page)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();

            _backgroundColor = page.BackgroundColor;

            page.Animate("backgroundFade", d =>
            {
                page.BackgroundColor = GetColor(d);
            }, _backgroundColor.A, 0, length: Duration, finished: (d, b) =>
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
