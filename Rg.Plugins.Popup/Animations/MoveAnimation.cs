using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    public class MoveAnimation : FadeBackgroundAnimation
    {
        private double _defaultTranslationX;
        private double _defaultTranslationY;

        public MoveAnimationOptions PositionIn { get; set; }
        public MoveAnimationOptions PositionOut { get; set; }

        public MoveAnimation(): this(MoveAnimationOptions.Bottom, MoveAnimationOptions.Bottom) {}

        public MoveAnimation(MoveAnimationOptions positionIn, MoveAnimationOptions positionOut)
        {
            PositionIn = positionIn;
            PositionOut = positionOut;

            DurationIn = DurationOut = 300;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
        }

        public override void Preparing(View content, PopupPage page)
        {
            base.Preparing(content, page);

            HidePage(page);

            if(content == null) return;

            UpdateDefaultTranslations(content);
        }

        public override void Disposing(View content, PopupPage page)
        {
            base.Disposing(content, page);

            ShowPage(page);

            if (content == null) return;

            content.TranslationX = _defaultTranslationX;
            content.TranslationY = _defaultTranslationY;
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Appearing(content, page));

            if (content != null)
            {
                var topOffset = GetTopOffset(content, page);
                var leftOffset = GetLeftOffset(content, page);

                if (PositionIn == MoveAnimationOptions.Top)
                {
                    content.TranslationY = -topOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationY = topOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Left)
                {
                    content.TranslationX = -leftOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Right)
                {
                    content.TranslationX = leftOffset;
                }

                taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
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
                UpdateDefaultTranslations(content);

                var topOffset = GetTopOffset(content, page);
                var leftOffset = GetLeftOffset(content, page);

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

        private void UpdateDefaultTranslations(View content)
        {
            _defaultTranslationX = content.TranslationX;
            _defaultTranslationY = content.TranslationY;
        }
    }
}
