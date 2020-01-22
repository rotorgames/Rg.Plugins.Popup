using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    public class ScaleAnimation : FadeAnimation
    {
        private double _defaultScale;
        private double _defaultOpacity;
        private double _defaultTranslationX;
        private double _defaultTranslationY;

        public double ScaleIn { get; set; } = 0.8;
        public double ScaleOut { get; set; } = 0.8;

        public MoveAnimationOptions PositionIn { get; set; }
        public MoveAnimationOptions PositionOut { get; set; }

        public ScaleAnimation():this(MoveAnimationOptions.Center, MoveAnimationOptions.Center) {}

        public ScaleAnimation(MoveAnimationOptions positionIn, MoveAnimationOptions positionOut)
        {
            PositionIn = positionIn;
            PositionOut = positionOut;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;

            if (PositionIn != MoveAnimationOptions.Center) DurationIn = 500;
            if (PositionOut != MoveAnimationOptions.Center) DurationOut = 500;
        }

        public override void Preparing(View content, PopupPage page)
        {
            if(HasBackgroundAnimation) base.Preparing(content, page);

            HidePage(page);

            if(content == null) return;

            UpdateDefaultProperties(content);

            if(!HasBackgroundAnimation) content.Opacity = 0;
        }

        public override void Disposing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation) base.Disposing(content, page);

            ShowPage(page);

            if(content == null) return;

            content.Scale = _defaultScale;
            content.Opacity = _defaultOpacity;
            content.TranslationX = _defaultTranslationX;
            content.TranslationY = _defaultTranslationY;
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Appearing(content, page));

            if (content != null)
            {
                var topOffset = GetTopOffset(content, page) * ScaleIn;
                var leftOffset = GetLeftOffset(content, page) * ScaleIn;

                taskList.Add(Scale(content, EasingIn, ScaleIn, _defaultScale, true));

                if (PositionIn == MoveAnimationOptions.Top)
                {
                    content.TranslationY = -topOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationY = topOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Left)
                {
                    content.TranslationX = -leftOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Right)
                {
                    content.TranslationX = leftOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
            }

            ShowPage(page);

            await Task.WhenAll(taskList);
        }

        public async override Task Disappearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Disappearing(content, page));

            if (content != null)
            {
                UpdateDefaultProperties(content);

                var topOffset = GetTopOffset(content, page) * ScaleOut;
                var leftOffset = GetLeftOffset(content, page) * ScaleOut;

                taskList.Add(Scale(content, EasingOut, _defaultScale, ScaleOut, false));

                if (PositionOut == MoveAnimationOptions.Top)
                {
                    taskList.Add(content.TranslateTo(_defaultTranslationX, -topOffset, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Bottom)
                {
                    taskList.Add(content.TranslateTo(_defaultTranslationX, topOffset, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Left)
                {
                    taskList.Add(content.TranslateTo(-leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Right)
                {
                    taskList.Add(content.TranslateTo(leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
            }

            await Task.WhenAll(taskList);
        }

        private Task Scale(View content, Easing easing, double start, double end, bool isAppearing)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, start, end,
            easing: easing,
            length: isAppearing ? DurationIn : DurationOut,
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        private void UpdateDefaultProperties(View content)
        {
            _defaultScale = content.Scale;
            _defaultOpacity = content.Opacity;
            _defaultTranslationX = content.TranslationX;
            _defaultTranslationY = content.TranslationY;
        }
    }
}
