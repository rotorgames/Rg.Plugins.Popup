using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class ScaleAnimation : FadeContentAnimation
    {
        private MoveAnimationsName _animationStartName;
        private MoveAnimationsName _animationEndName;

        private double _startScale;

        public double StartScaleIn { get; set; } = 0.8;
        public double StartScaleOut { get; set; } = 1.2;

        public ScaleAnimation(MoveAnimationsName startName, MoveAnimationsName endName, ScaleAnimationsName scaleName)
        {
            _animationStartName = startName;
            _animationEndName = endName;
            if (scaleName == ScaleAnimationsName.Up)
            {
                _startScale = StartScaleIn;
            }
            else
            {
                _startScale = StartScaleOut;
            }
        }

        public override void Preparing(View content, PopupPage page)
        {
            base.Preparing(content, page);

            if (_animationStartName != MoveAnimationsName.Center)
            {
                Duration = 500;
            }
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
            content.Scale = _startScale;
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();
            
            taskList.Add(base.Appearing(content, page));
            
            var topOffset = GetTopOffset(content, page) * StartScaleIn;
            var leftOffset = GetLeftOffset(content, page) * StartScaleIn;
            
            taskList.Add(Scale(content, EasingIn, _startScale, 1));

            if (_animationStartName == MoveAnimationsName.Top)
            {
                content.TranslationY = -topOffset;
                taskList.Add(content.TranslateTo(0, 0, Duration, EasingIn));
            }
            else if(_animationStartName == MoveAnimationsName.Bottom)
            {
                content.TranslationY = topOffset;
                taskList.Add(content.TranslateTo(0, 0, Duration, EasingIn));
            }
            else if (_animationStartName == MoveAnimationsName.Left)
            {
                content.TranslationX = -leftOffset;
                taskList.Add(content.TranslateTo(0, 0, Duration, EasingIn));
            }
            else if (_animationStartName == MoveAnimationsName.Right)
            {
                content.TranslationX = leftOffset;
                taskList.Add(content.TranslateTo(0, 0, Duration, EasingIn));
            }

            await Task.WhenAll(taskList);
        }

        public async override Task Disappearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Disappearing(content, page));
            
            var topOffset = GetTopOffset(content, page) * StartScaleOut;
            var leftOffset = GetLeftOffset(content, page) * StartScaleOut;
            
            taskList.Add(Scale(content, EasingOut, 1, _startScale));

            if (_animationEndName == MoveAnimationsName.Top)
            {
                taskList.Add(content.TranslateTo(0, -topOffset, Duration, EasingIn));
            }
            else if (_animationEndName == MoveAnimationsName.Bottom)
            {
                taskList.Add(content.TranslateTo(0, topOffset, Duration, EasingIn));
            }
            else if (_animationEndName == MoveAnimationsName.Left)
            {
                taskList.Add(content.TranslateTo(-leftOffset, 0, Duration, EasingIn));
            }
            else if (_animationEndName == MoveAnimationsName.Right)
            {
                taskList.Add(content.TranslateTo(leftOffset, 0, Duration, EasingIn));
            }

            await Task.WhenAll(taskList);
        }

        private Task Scale(View content, Easing easing, double start, double end)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, start, end,
            easing: easing,
            length: Duration,
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }
    }
}
