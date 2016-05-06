using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class MoveAnimation : BaseAnimation
    {
        private MoveAnimationsName _animationStartName;
        private MoveAnimationsName _animationEndName;
        public MoveAnimation(MoveAnimationsName animationStartName, MoveAnimationsName animationEndName)
        {
            _animationStartName = animationStartName;
            _animationEndName = animationEndName;
        }
        public override void Preparing(View content, PopupPage page)
        {
            content.Opacity = 0;
            Time = 300;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
        }

        public override void Disposing(View content, PopupPage page)
        {
            
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            content.Opacity = 1;
            var topOffset = GetTopOffset(content, page);
            var leftOffset = GetLeftOffset(content, page);

            if (_animationStartName == MoveAnimationsName.Top)
            {
                content.TranslationY = -topOffset;
            }
            else if (_animationStartName == MoveAnimationsName.Bottom)
            {
                content.TranslationY = topOffset;
            }
            else if (_animationStartName == MoveAnimationsName.Left)
            {
                content.TranslationX = -leftOffset;
            }
            else if (_animationStartName == MoveAnimationsName.Right)
            {
                content.TranslationX = leftOffset;
            }

            taskList.Add(content.TranslateTo(0, 0, Time, EasingIn));

            await Task.WhenAll(taskList);
        }

        public async override Task Disappearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            var topOffset = GetTopOffset(content, page);
            var leftOffset = GetLeftOffset(content, page);

            if (_animationEndName == MoveAnimationsName.Top)
            {
                taskList.Add(content.TranslateTo(0, -topOffset, Time, EasingOut));
            }
            else if (_animationEndName == MoveAnimationsName.Bottom)
            {
                taskList.Add(content.TranslateTo(0, topOffset, Time, EasingOut));
            }
            else if (_animationEndName == MoveAnimationsName.Left)
            {
                taskList.Add(content.TranslateTo(-leftOffset, 0, Time, EasingOut));
            }
            else if (_animationEndName == MoveAnimationsName.Right)
            {
                taskList.Add(content.TranslateTo(leftOffset, 0, Time, EasingOut));
            }

            await Task.WhenAll(taskList);
        }
    }
}
