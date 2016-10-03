using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Platform.Renderers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Pages
{
    [RenderWith(typeof(_PopupPageRenderer))]
    public class PopupPage : ContentPage
    {
        #region Private Fields

        private Action _appearingAction;

        #endregion

        #region Internal Fields

        internal bool IsAnimate;

        #endregion

        #region Events

        public event EventHandler BackgroundClicked;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty IsSystemPaddingProperty = BindableProperty.Create(nameof(IsSystemPadding), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(IPopupAnimation), typeof(PopupPage));
        public static readonly BindableProperty IsCloseOnBackgroundClickProperty = BindableProperty.Create(nameof(IsCloseOnBackgroundClick), typeof(bool), typeof(PopupPage), true);

        #endregion

        #region Properties

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public bool IsSystemPadding
        {
            get { return (bool)GetValue(IsSystemPaddingProperty); }
            set { SetValue(IsSystemPaddingProperty, value); }
        }

        public IPopupAnimation Animation
        {
            get { return (IPopupAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public Thickness SystemPadding
        {
            get { return DependencyService.Get<IScreenHelper>().ScreenOffsets; }
        }

        public bool IsCloseOnBackgroundClick
        {
            get { return (bool)GetValue(IsCloseOnBackgroundClickProperty); }
            set { SetValue(IsCloseOnBackgroundClickProperty, value); }
        }

        #endregion

        #region Main Methods

        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = new ScaleAnimation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _appearingAction?.Invoke();
            _appearingAction = null;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Padding) || propertyName == nameof(IsSystemPadding))
            {
                ForceLayout();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        #endregion

        #region Helper Methods

        internal void ExecuteWhenAppearingOnce(Action action)
        {
            _appearingAction = action;
        }

        #endregion

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
        }

        internal void DisposingAnimation()
        {
            if (IsAnimating) Animation?.Disposing(Content, this);
        }

        internal async Task AppearingAnimation()
        {
            IsAnimate = true;

            if (IsAnimating && Animation != null)
                await Animation.Appearing(Content, this);

            IsAnimate = false;

            OnAppearingAnimationEnd();
        }

        internal async Task DisappearingAnimation()
        {
            IsAnimate = true;

            if (IsAnimating && Animation != null)
                await Animation.Disappearing(Content, this);

            IsAnimate = false;

            OnDisappearingAnimationEnd();
        }

        protected virtual void OnAppearingAnimationEnd() { }

        protected virtual void OnDisappearingAnimationEnd() { }

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
