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
                Time = 500;
            }
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
            content.Scale = _startScale;
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            base.Appearing(content, page);
            var topOffset = GetTopOffset(content, page) * StartScaleIn;
            var leftOffset = GetLeftOffset(content, page) * StartScaleIn;
            if (_animationStartName == MoveAnimationsName.Center)
            {
                await Scale(content, EasingIn, _startScale, 1);
            }
            else
            {
                Scale(content, EasingIn, _startScale, 1);
            }

            if (_animationStartName == MoveAnimationsName.Top)
            {
                content.TranslationY = -topOffset;
                await content.TranslateTo(0, 0, Time, EasingIn);
            }
            else if(_animationStartName == MoveAnimationsName.Bottom)
            {
                content.TranslationY = topOffset;
                await content.TranslateTo(0, 0, Time, EasingIn);
            }
            else if (_animationStartName == MoveAnimationsName.Left)
            {
                content.TranslationX = -leftOffset;
                await content.TranslateTo(0, 0, Time, EasingIn);
            }
            else if (_animationStartName == MoveAnimationsName.Right)
            {
                content.TranslationX = leftOffset;
                await content.TranslateTo(0, 0, Time, EasingIn);
            }
        }

        public async override Task Disappearing(View content, PopupPage page)
        {
            base.Disappearing(content, page);
            var topOffset = GetTopOffset(content, page) * StartScaleOut;
            var leftOffset = GetLeftOffset(content, page) * StartScaleOut;
            if (_animationEndName == MoveAnimationsName.Center)
            {
                await Scale(content, EasingOut, 1, _startScale);
            }
            else
            {
                Scale(content, EasingOut, 1, _startScale);
            }

            if (_animationEndName == MoveAnimationsName.Top)
            {
                await content.TranslateTo(0, -topOffset, Time, EasingIn);
            }
            else if (_animationEndName == MoveAnimationsName.Bottom)
            {
                await content.TranslateTo(0, topOffset, Time, EasingIn);
            }
            else if (_animationEndName == MoveAnimationsName.Left)
            {
                await content.TranslateTo(-leftOffset, 0, Time, EasingIn);
            }
            else if (_animationEndName == MoveAnimationsName.Right)
            {
                await content.TranslateTo(leftOffset, 0, Time, EasingIn);
            }
        }

        private Task Scale(View content, Easing easing, double start, double end)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, start, end,
            easing: easing,
            length: Time,
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }
    }
}
