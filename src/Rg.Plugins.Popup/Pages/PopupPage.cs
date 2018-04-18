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
        #region Private

        private const string IsAnimatingObsoleteText = 
            nameof(IsAnimating) + 
            " is obsolute as of v1.1.5. Please use "
            +nameof(IsAnimationEnabled) + 
            " instead. See more info: "
            +Config.MigrationV1_0_xToV1_1_xUrl;

        #endregion

        #region Internal Properties

        internal bool IsBeingDismissed { get; set; }

        #endregion

        #region Events

        public event EventHandler BackgroundClicked;

        #endregion

        #region Bindable Properties

        [Obsolete(IsAnimatingObsoleteText)]
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(PopupPage), true);

        [Obsolete(IsAnimatingObsoleteText)]
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public static readonly BindableProperty IsAnimationEnabledProperty = BindableProperty.Create(nameof(IsAnimationEnabled), typeof(bool), typeof(PopupPage), true);

        public bool IsAnimationEnabled
        {
            get { return (bool)GetValue(IsAnimationEnabledProperty); }
            set { SetValue(IsAnimationEnabledProperty, value); }
        }

        public static readonly BindableProperty HasSystemPaddingProperty = BindableProperty.Create(nameof(HasSystemPadding), typeof(bool), typeof(PopupPage), true);

        public bool HasSystemPadding
        {
            get { return (bool)GetValue(HasSystemPaddingProperty); }
            set { SetValue(HasSystemPaddingProperty, value); }
        }

        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(IPopupAnimation), typeof(PopupPage));

        public IPopupAnimation Animation
        {
            get { return (IPopupAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public static readonly BindableProperty SystemPaddingProperty = BindableProperty.Create(nameof(SystemPadding), typeof(Thickness), typeof(PopupPage), default(Thickness), BindingMode.OneWayToSource);

        public Thickness SystemPadding
        {
            get { return (Thickness)GetValue(SystemPaddingProperty); }
            private set { SetValue(SystemPaddingProperty, value); }
        }

        public static readonly BindableProperty CloseWhenBackgroundIsClickedProperty = BindableProperty.Create(nameof(CloseWhenBackgroundIsClicked), typeof(bool), typeof(PopupPage), true);

        public bool CloseWhenBackgroundIsClicked
        {
            get { return (bool)GetValue(CloseWhenBackgroundIsClickedProperty); }
            set { SetValue(CloseWhenBackgroundIsClickedProperty, value); }
        }

        public static readonly BindableProperty BackgroundInputTransparentProperty = BindableProperty.Create(nameof(BackgroundInputTransparent), typeof(bool), typeof(PopupPage), false);

        public bool BackgroundInputTransparent
        {
            get { return (bool)GetValue(BackgroundInputTransparentProperty); }
            set { SetValue(BackgroundInputTransparentProperty, value); }
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
                case nameof(IsAnimating):
                    IsAnimationEnabled = IsAnimating;
                    break;
                case nameof(IsAnimationEnabled):
                    IsAnimating = IsAnimationEnabled;
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
            if (IsAnimationEnabled)
                Animation?.Preparing(Content, this);
        }

        internal void DisposingAnimation()
        {
            if (IsAnimationEnabled)
                Animation?.Disposing(Content, this);
        }

        internal async Task AppearingAnimation()
        {
            OnAppearingAnimationBegin();
            await OnAppearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Appearing(Content, this);

            OnAppearingAnimationEnd();
            await OnAppearingAnimationEndAsync();
        }

        internal async Task DisappearingAnimation()
        {
            OnDisappearingAnimationBegin();
            await OnDisappearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Disappearing(Content, this);

            OnDisappearingAnimationEnd();
            await OnDisappearingAnimationEndAsync();
        }

        #endregion

        #region Override Animation Methods

        protected virtual void OnAppearingAnimationBegin()
        {
        }

        protected virtual void OnAppearingAnimationEnd()
        {
        }

        protected virtual void OnDisappearingAnimationBegin()
        {
        }

        protected virtual void OnDisappearingAnimationEnd()
        {
        }

        protected virtual Task OnAppearingAnimationBeginAsync()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnAppearingAnimationEndAsync()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnDisappearingAnimationBeginAsync()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnDisappearingAnimationEndAsync()
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

        internal void SetSystemPadding(Thickness systemPadding, bool forceLayout = true)
        {
            var systemPaddingWasChanged = SystemPadding != systemPadding;
            SystemPadding = systemPadding;

            if(systemPaddingWasChanged && forceLayout)
                ForceLayout();
        }

        #endregion
    }
}
