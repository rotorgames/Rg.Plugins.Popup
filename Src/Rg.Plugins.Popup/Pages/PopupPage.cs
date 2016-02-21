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
        public static readonly BindableProperty IsSystemPaddingProperty = BindableProperty.Create<PopupPage, bool>(p => p.IsSystemPadding, true);
        public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create<PopupPage, AnimationsName>(p => p.AnimationName, AnimationsName.ScaleCenterUp);
        public static new readonly BindableProperty PaddingProperty = BindableProperty.Create<PopupPage, Thickness>(p => p.Padding, new Thickness());

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

        public new Thickness Padding
        {
            get { return (Thickness) GetValue(PaddingProperty); }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        public Thickness SystemPadding
        {
            get { return DependencyService.Get<IScreenHelper>().ScreenOffsets; }
        }

        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = AnimationServices.GeAnimation(AnimationName);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(Padding) || propertyName == nameof(IsSystemPadding))
            {
                base.Padding = GetPadding();
            }
            else
            {
                base.OnPropertyChanged(propertyName);
            }

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

        #region Size Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            base.Padding = GetPadding();
            if (Device.OS == TargetPlatform.Android)
            {
                Layout(DependencyService.Get<IScreenHelper>().ScreenSize);
            }
        }

        private Thickness GetPadding()
        {
            if (!IsSystemPadding)
            {
                return Padding;
            }
            var userPadding = Padding;
            var systemPadding = DependencyService.Get<IScreenHelper>().ScreenOffsets;
            var padding = new Thickness
            {
                Top = userPadding.Top + systemPadding.Top,
                Bottom = userPadding.Bottom + systemPadding.Bottom,
                Left = userPadding.Left + systemPadding.Left,
                Right = userPadding.Right + systemPadding.Right,
            };
            return padding;
        }

        #endregion

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
