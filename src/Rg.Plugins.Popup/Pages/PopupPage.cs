using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Pages
{
    public class PopupPage : ContentPage
    {
        private IPopupAnimation _animation;
        private IPopupAnimation _backgroundAnimation;

        internal bool IsAnimate;

        public event EventHandler BackgroundClicked; 

        public static readonly BindableProperty IsBackgroundAnimatingProperty = BindableProperty.Create(nameof(IsBackgroundAnimating), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty IsSystemPaddingProperty = BindableProperty.Create(nameof(IsSystemPadding), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create(nameof(AnimationName), typeof(AnimationsName), typeof(PopupPage), AnimationsName.ScaleCenterUp);
        public static readonly BindableProperty IsCloseOnBackgroundClickProperty = BindableProperty.Create(nameof(IsCloseOnBackgroundClick), typeof(bool), typeof(PopupPage), true);

        public bool IsBackgroundAnimating
        {
            get
            {
                return (bool) GetValue(IsBackgroundAnimatingProperty);
                
            }
            set { SetValue(IsBackgroundAnimatingProperty, value);}
        }

        public bool IsAnimating
        {
            get
            {
                return (bool)GetValue(IsAnimatingProperty);

            }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public bool IsSystemPadding
        {
            get
            {
                return (bool)GetValue(IsSystemPaddingProperty);

            }
            set { SetValue(IsSystemPaddingProperty, value); }
        }

        public AnimationsName AnimationName
        {
            get
            {
                return (AnimationsName)GetValue(AnimationNameProperty);

            }
            set
            {
                SetValue(AnimationNameProperty, value);
            }
        }

        protected IPopupAnimation Animation
        {
            get { return _animation; }
            set
            {
                if(AnimationName != AnimationsName.Others) AnimationName = AnimationsName.Others;
                _animation = value;
            }
        }

        public Thickness SystemPadding
        {
            get { return DependencyService.Get<IScreenHelper>().ScreenOffsets; }
        }

        public bool IsCloseOnBackgroundClick
        {
            get { return (bool) GetValue(IsCloseOnBackgroundClickProperty); }
            set { SetValue(IsCloseOnBackgroundClickProperty, value);}
        }

        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = AnimationService.GeAnimation(AnimationName);
            
            _backgroundAnimation = new FadeBackgroundAnimation();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(Padding) || propertyName == nameof(IsSystemPadding))
            {
                ForceLayout();
            }
            else
            {
                base.OnPropertyChanged(propertyName);
            }

            if (propertyName == nameof(AnimationName))
            {
                _animation = AnimationService.GeAnimation(AnimationName);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        #region Size Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.Windows)
            {
                Layout(DependencyService.Get<IScreenHelper>().ScreenSize);
            }
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!IsSystemPadding)
            {
                base.LayoutChildren(x, y, width, height);
                return;
            }

            var systemPadding = SystemPadding;

            x += systemPadding.Left;
            y += systemPadding.Top;
            width -= systemPadding.Left + systemPadding.Right;
            height -= systemPadding.Top + systemPadding.Bottom;

            base.LayoutChildren(x, y, width, height);
        }

        #endregion

        #region Animation Methods

        internal void PreparingAnimation()
        {
            if (IsAnimating) Animation?.Preparing(Content, this);
            if (IsBackgroundAnimating) _backgroundAnimation.Preparing(Content, this);
        }

        internal void DisposingAnimation()
        {
            if (IsAnimating) Animation?.Disposing(Content, this);
            if (IsBackgroundAnimating) _backgroundAnimation.Disposing(Content, this);
        }

        internal async Task AppearingAnimation()
        {
            IsAnimate = true;
            var taskList = new List<Task>();

            if (IsAnimating && Animation != null)
            {
                _backgroundAnimation.Duration = Animation.Duration;
                taskList.Add(Animation.Appearing(Content, this));
            }
            if (IsBackgroundAnimating)
            {
                taskList.Add(_backgroundAnimation.Appearing(Content, this));
            }

            await Task.WhenAll(taskList);
            IsAnimate = false;
        }

        internal async Task DisappearingAnimation()
        {
            IsAnimate = true;
            var taskList = new List<Task>();

            if (IsAnimating && Animation != null)
            {
                _backgroundAnimation.Duration = Animation.Duration;
                taskList.Add(Animation.Disappearing(Content, this));
            }
            if (IsBackgroundAnimating)
            {
                taskList.Add(_backgroundAnimation.Disappearing(Content, this));
            }

            await Task.WhenAll(taskList);
            IsAnimate = false;
        }

        #endregion

        #region Send Methods

        internal async void SendBackgroundClick()
        {
            BackgroundClicked?.Invoke(this, EventArgs.Empty);

            if (IsCloseOnBackgroundClick)
            {
                await PopupNavigation.PopAsync();
            }
        }

        #endregion
    }
}
