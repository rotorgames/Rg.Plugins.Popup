using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Pages
{
    public class PopupPage : ContentPage
    {
        private IPopupAnimation _animation;

        public static readonly BindableProperty IsBackgroundAnimatingProperty = BindableProperty.Create<PopupPage, bool>(p => p.IsBackgroundAnimating, true);
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create<PopupPage, bool>(p => p.IsAnimating, true);
        public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create<PopupPage, AnimationsName>(p => p.AnimationName, AnimationsName.ScaleCenterUp);

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

        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = AnimationServices.GeAnimation(AnimationName);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(AnimationName))
            {
                _animation = AnimationServices.GeAnimation(AnimationName);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.PopAsync();
            return true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Device.OS == TargetPlatform.Android)
            {
                Layout(DependencyService.Get<IScreenHelper>().ScreenSize);
            }
        }

        #region Animation Methods

        internal void PreparingAnimation()
        {
            if (IsAnimating && Animation != null)
            {
                Animation.Preparing(Content, this);
            }
        }

        internal void DisposingAnimation()
        {
            if (IsAnimating && Animation != null)
            {
                Animation.Disposing(Content, this);
            }
        }

        internal async Task AppearingAnimation()
        {
            if (IsAnimating && Animation != null)
            {
                await Animation.Appearing(Content, this);
            }
        }

        internal async Task DisappearingAnimation()
        {
            if (IsAnimating && Animation != null)
            {
                await Animation.Disappearing(Content, this);
            }
        }

        #endregion
    }
}
