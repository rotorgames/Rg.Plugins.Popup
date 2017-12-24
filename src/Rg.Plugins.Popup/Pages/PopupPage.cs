using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Pages
{
    public class PopupPage : ContentPage
    {
        #region Internal Properties

        internal bool IsBeingDismissed { get; set; }

        #endregion

        #region Events

        public event EventHandler BackgroundClicked;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty HasSystemPaddingProperty = BindableProperty.Create(nameof(HasSystemPadding), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(IPopupAnimation), typeof(PopupPage));
        public static readonly BindableProperty CloseWhenBackgroundIsClickedProperty = BindableProperty.Create(nameof(CloseWhenBackgroundIsClicked), typeof(bool), typeof(PopupPage), true);
        public static readonly BindableProperty SystemPaddingProperty = BindableProperty.Create(nameof(SystemPadding), typeof(Thickness), typeof(PopupPage), default(Thickness), BindingMode.OneWayToSource);

        #endregion

        #region Properties

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public bool HasSystemPadding
        {
            get { return (bool)GetValue(HasSystemPaddingProperty); }
            set { SetValue(HasSystemPaddingProperty, value); }
        }

        public IPopupAnimation Animation
        {
            get { return (IPopupAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public Thickness SystemPadding
        {
            get { return (Thickness)GetValue(SystemPaddingProperty); }
            private set { SetValue(SystemPaddingProperty, value); }
        }

        public bool CloseWhenBackgroundIsClicked
        {
            get { return (bool)GetValue(CloseWhenBackgroundIsClickedProperty); }
            set { SetValue(CloseWhenBackgroundIsClickedProperty, value); }
        }

        #endregion

        #region Main Methods

        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = new ScaleAnimation();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(HasSystemPadding):
                    ForceLayout();
                    break;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        #endregion

        #region Size Methods

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!HasSystemPadding)
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
            if (IsAnimating && Animation != null)
                await Animation.Appearing(Content, this);

            await OnAppearingAnimationEnd();
        }

        internal async Task DisappearingAnimation()
        {
            await OnDisappearingAnimationBegin();

            if (IsAnimating && Animation != null)
                await Animation.Disappearing(Content, this);
        }

        #endregion

        #region Override Animation Methods

        protected virtual Task OnAppearingAnimationEnd()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Task.FromResult(0);
        }

        #endregion

        #region Background Click

        protected virtual bool OnBackgroundClicked()
        {
            return CloseWhenBackgroundIsClicked;
        }

        #endregion

        #region Internal Methods

        internal async void SendBackgroundClick()
        {
            BackgroundClicked?.Invoke(this, EventArgs.Empty);

            var isClose = OnBackgroundClicked();
            if (isClose)
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        internal void SetSystemPadding(Thickness systemPadding)
        {
            SystemPadding = systemPadding;
            ForceLayout();
        }

        #endregion
    }
}
