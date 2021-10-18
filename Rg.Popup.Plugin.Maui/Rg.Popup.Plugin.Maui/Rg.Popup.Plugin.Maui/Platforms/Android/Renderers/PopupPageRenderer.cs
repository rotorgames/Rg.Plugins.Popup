using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;

using Microsoft.Maui.Controls;
using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Rg.Plugins.Popup.Pages;
using Microsoft.Maui.Handlers;
using System.Threading.Tasks;

namespace Rg.Plugins.Popup.Pages
{

    public class PopupPageHandler : ContentViewHandler
    {
        public PopupPageHandler()
        {
            try
            { //This needs to be changed around to be proper
                this.SetMauiContext(new MauiContext(Microsoft.Maui.MauiApplication.Current.Services, Microsoft.Maui.MauiApplication.Current.ApplicationContext));
                Task.Run(async () =>
                {
                    while (this.VirtualView == null)
                    {
                        await Task.Delay(100);
                    }

                    this.NativeView.LayoutChange += PopupPage_LayoutChange;
                });
               // this.NativeView.LayoutChange += PopupPage_LayoutChange;

            }
            catch (Exception ex )
            {

                throw;
            }
                
        }

        private void PopupPage_LayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
        {

            var activity = Microsoft.Maui.Essentials.Platform.CurrentActivity;

            Microsoft.Maui.Thickness systemPadding;
            var keyboardOffset = 0d;

            var decoreView = activity.Window.DecorView;
            var decoreHeight = decoreView.Height;
            var decoreWidth = decoreView.Width;

            using var visibleRect = new Android.Graphics.Rect();

            decoreView.GetWindowVisibleDisplayFrame(visibleRect);
            //Proving that i'm influencing the position using changes here. 
            this.VirtualView.CrossPlatformArrange(new Rectangle(Context.FromPixels(e.Top/2), Context.FromPixels(e.Left/2), Context.FromPixels(e.Right/2), Context.FromPixels(e.Bottom/2)));

            //this.SystemPadding = systemPadding;
            //this.KeyboardOffset = keyboardOffset;

            //this.Layout(new Rectangle(, Context.FromPixels(t), Context.FromPixels(r), Context.FromPixels(b)));
            //else
            //   CurrentElement.ForceLayout();

            //base.OnLayout(changed, l, t, r, b);
        }

        public PopupPageHandler(IPropertyMapper? mapper = null) : base(mapper)
        {
        }

        protected PopupPageHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }

        public override bool NeedsContainer => base.NeedsContainer;

        public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        public override void Invoke(string command, object? args)
        {
            base.Invoke(command, args);
        }

        public override void NativeArrange(Rectangle frame)
        {
            base.NativeArrange(frame);
        }

        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);
        }

        public override void UpdateValue(string property)
        {
            base.UpdateValue(property);
        }

        protected override void ConnectHandler(ContentViewGroup nativeView)
        {
            base.ConnectHandler(nativeView);
        }

        protected override ContentViewGroup CreateNativeView()
        {
            return base.CreateNativeView();
        }

        protected override void DisconnectHandler(ContentViewGroup nativeView)
        {
            base.DisconnectHandler(nativeView);
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }
    }

    public partial class PopupPage : ContentPage
    {
        partial void ChangedHandler(object sender, EventArgs e)
        {
            ((sender as ContentPage).Handler.NativeView as Android.Views.View).LayoutChange += PopupPage_LayoutChange;
        }

        private void PopupPage_LayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
        {

            var activity = Microsoft.Maui.Essentials.Platform.CurrentActivity;

            Microsoft.Maui.Thickness systemPadding;
            var keyboardOffset = 0d;

            var decoreView = activity.Window.DecorView;
            var decoreHeight = decoreView.Height;
            var decoreWidth = decoreView.Width;

            using var visibleRect = new Android.Graphics.Rect();

            decoreView.GetWindowVisibleDisplayFrame(visibleRect);

            //this.SystemPadding = systemPadding;
            this.KeyboardOffset = keyboardOffset;

            //this.Layout(new Rectangle(, Context.FromPixels(t), Context.FromPixels(r), Context.FromPixels(b)));
            //else
             //   CurrentElement.ForceLayout();

            //base.OnLayout(changed, l, t, r, b);
        }

        partial void ChangingHandler(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
                //(e.OldHandler.NativeView as Android.Views.View).FocusChange -= OnFocusChange;
            }
        }

        /*
        void OnFocusChange(object sender, EventArgs e)
        {
            var nativeView = sender as AppCompatEditText;

            if (nativeView.IsFocused)
                nativeView.SetBackgroundColor(Colors.LightPink.ToNative());
            else
                nativeView.SetBackgroundColor(Colors.White.ToNative());
        }
        */


    }
    /*
    public class PopupPageRenderer : PageRenderer
    {
        private readonly RgGestureDetectorListener _gestureDetectorListener;
        private readonly GestureDetector _gestureDetector;
        private DateTime _downTime;
        private Microsoft.Maui.Graphics.Point _downPosition;
        private bool _disposed;

        private PopupPage CurrentElement => (PopupPage)Element;

        #region Main Methods

        public PopupPageRenderer(Context context) : base(context)
        {
            //_gestureDetectorListener = new RgGestureDetectorListener();

            //_gestureDetectorListener.Clicked += OnBackgroundClick;

            //_gestureDetector = new GestureDetector(Context, _gestureDetectorListener);
        }

        protected override void Dispose(bool disposing)
        {
            
            if (disposing)
            {
                _disposed = true;

                _gestureDetectorListener.Clicked -= OnBackgroundClick;
                _gestureDetectorListener.Dispose();
                _gestureDetector.Dispose();
            }
            
            base.Dispose(disposing);
            
        }

        #endregion

        #region Layout Methods

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            
            var activity = Microsoft.Maui.Essentials.Platform.CurrentActivity;

            Microsoft.Maui.Thickness systemPadding;
            var keyboardOffset = 0d;

            var decoreView = activity.Window.DecorView;
            var decoreHeight = decoreView.Height;
            var decoreWidht = decoreView.Width;

            using var visibleRect = new Android.Graphics.Rect();

            decoreView.GetWindowVisibleDisplayFrame(visibleRect);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {

                //var screenRealEstate = Microsoft.Maui.Essentials.DeviceDisplay.MainDisplayInfo
                using var screenRealSize = new Android.Graphics.Point();
                activity.WindowManager.DefaultDisplay.GetRealSize(screenRealSize);

                var windowInsets = RootWindowInsets;
                var bottomPadding = Math.Min(windowInsets.StableInsetBottom, windowInsets.SystemWindowInsetBottom);

                if (screenRealSize.Y - visibleRect.Bottom > windowInsets.StableInsetBottom)
                {
                    keyboardOffset = Context.FromPixels(screenRealSize.Y - visibleRect.Bottom);
                }

                systemPadding = new Microsoft.Maui.Thickness
                {
                    Left = Context.FromPixels(windowInsets.SystemWindowInsetLeft),
                    Top = Context.FromPixels(windowInsets.SystemWindowInsetTop),
                    Right = Context.FromPixels(windowInsets.SystemWindowInsetRight),
                    Bottom = Context.FromPixels(bottomPadding)
                };
            }
            else
            {
                using var screenSize = new Android.Graphics.Point();

                activity.WindowManager.DefaultDisplay.GetSize(screenSize);

                var keyboardHeight = 0d;

                if (visibleRect.Bottom < screenSize.Y)
                {
                    keyboardHeight = screenSize.Y - visibleRect.Bottom;
                    keyboardOffset = Context.FromPixels(decoreHeight - visibleRect.Bottom);
                }

                systemPadding = new Microsoft.Maui.Thickness
                {
                    Left = Context.FromPixels(visibleRect.Left),
                    Top = Context.FromPixels(visibleRect.Top),
                    Right = Context.FromPixels(decoreWidht - visibleRect.Right),
                    Bottom = Context.FromPixels(decoreHeight - visibleRect.Bottom - keyboardHeight)
                };
            }

            CurrentElement.SetValue(PopupPage.SystemPaddingProperty, systemPadding);
            CurrentElement.SetValue(PopupPage.KeyboardOffsetProperty, keyboardOffset);

            if (changed)
                CurrentElement.Layout(new Rectangle(Context.FromPixels(l), Context.FromPixels(t), Context.FromPixels(r), Context.FromPixels(b)));
            else
                CurrentElement.ForceLayout();
            
            base.OnLayout(changed, l, t, r, b);
        }

        #endregion

        #region Life Cycle Methods

        protected override void OnAttachedToWindow()
        {
            //Context.HideKeyboard(Microsoft.Maui.Essentials.Platform.CurrentActivity.Window.DecorView);
            base.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            
            Microsoft.Maui.Controls.Device.StartTimer(TimeSpan.FromMilliseconds(0), () =>
            {
                Popup.Context.HideKeyboard(((Activity)Popup.Context).Window.DecorView);
                return false;
            });
            
            base.OnDetachedFromWindow();
        }

        protected override void OnWindowVisibilityChanged(ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);

            // It is needed because a size of popup has not updated on Android 7+. See #209
            //if (visibility == ViewStates.Visible)
                //RequestLayout();
        }

        #endregion

        #region Touch Methods

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            
            if (e.Action == MotionEventActions.Down)
            {
                _downTime = DateTime.UtcNow;
                _downPosition = new Microsoft.Maui.Graphics.Point(e.RawX, e.RawY);
            }
            if (e.Action != MotionEventActions.Up)
                return base.DispatchTouchEvent(e);

            if (_disposed)
                return false;

            View? currentFocus1 = Microsoft.Maui.Essentials.Platform.CurrentActivity.CurrentFocus;

            if (currentFocus1 is EditText)
            {
                View? currentFocus2 = Microsoft.Maui.Essentials.Platform.CurrentActivity.CurrentFocus;
                if (currentFocus1 == currentFocus2 && _downPosition.Distance(new Microsoft.Maui.Graphics.Point(e.RawX, e.RawY)) <= Context.ToPixels(20.0) && !(DateTime.UtcNow - _downTime > TimeSpan.FromMilliseconds(200.0)))
                {
                    int[] location = new int[2];
                    currentFocus1.GetLocationOnScreen(location);
                    float num1 = e.RawX + currentFocus1.Left - location[0];
                    float num2 = e.RawY + currentFocus1.Top - location[1];
                    if (!new Rectangle(currentFocus1.Left, currentFocus1.Top, currentFocus1.Width, currentFocus1.Height).Contains(num1, num2))
                    {
                        Context.HideKeyboard(currentFocus1);
                        currentFocus1.ClearFocus();
                    }
                }
            }

            if (_disposed)
                return false;

            var flag = base.DispatchTouchEvent(e);

            return flag;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (_disposed)
                return false;

            var baseValue = base.OnTouchEvent(e);

            _gestureDetector.OnTouchEvent(e);

            if (CurrentElement?.BackgroundInputTransparent == true)
            {
                if ((ChildCount > 0 && !IsInRegion(e.RawX, e.RawY, GetChildAt(0)!)) || ChildCount == 0)
                {
                    CurrentElement.SendBackgroundClick();
                    return false;
                }
            }

            return baseValue;
            
            return base.OnTouchEvent(e);
        }
        
        private void OnBackgroundClick(object? sender, MotionEvent e)
        {
            if (ChildCount == 0)
                return;

            var isInRegion = IsInRegion(e.RawX, e.RawY, GetChildAt(0)!);

            if (!isInRegion)
                CurrentElement.SendBackgroundClick();
        }

        // Fix for "CloseWhenBackgroundIsClicked not works on Android with Xamarin.Forms 2.4.0.280" #173
        private bool IsInRegion(float x, float y, View v)
        {
            var mCoordBuffer = new int[2];

            v.GetLocationOnScreen(mCoordBuffer);
            return mCoordBuffer[0] + v.Width > x &&    // right edge
                   mCoordBuffer[1] + v.Height > y &&   // bottom edge
                   mCoordBuffer[0] < x &&              // left edge
                   mCoordBuffer[1] < y;                // top edge
        }
        

        #endregion
    }
*/
}
