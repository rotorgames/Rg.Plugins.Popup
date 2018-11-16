using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Rg.Plugins.Popup.Droid.Gestures;
using Rg.Plugins.Popup.Droid.Renderers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Point = Xamarin.Forms.Point;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.Droid.Renderers
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private readonly RgGestureDetectorListener _gestureDetectorListener;
        private readonly GestureDetector _gestureDetector;
        private DateTime _downTime;
        private Point _downPosition;
        private bool _disposed;

        private PopupPage CurrentElement => (PopupPage) Element;

        #region Main Methods

        public PopupPageRenderer(Context context):base(context)
        {
            _gestureDetectorListener = new RgGestureDetectorListener();

            _gestureDetectorListener.Clicked += OnBackgroundClick;

            _gestureDetector = new GestureDetector(Context, _gestureDetectorListener);
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
            CurrentElement.BatchBegin();

            var systemPadding = GetSystemPadding();

            CurrentElement.SetSystemPadding(systemPadding, !changed);

            if(changed)
                CurrentElement.Layout(new Rectangle(Context.FromPixels(l), Context.FromPixels(t), Context.FromPixels(r), Context.FromPixels(b)));

            CurrentElement.BatchCommit();

            base.OnLayout(changed, l, t, r, b);
        }

        #endregion

        #region Size Methods

        private Thickness GetSystemPadding()
        {
            var decoreView = (FrameLayout)((Activity)Context).Window.DecorView;
            Rect visibleRect = new Rect();
            decoreView.GetWindowVisibleDisplayFrame(visibleRect);

            var decoreHeight = decoreView.Height;
            var decoreWidht = decoreView.Width;

            var result = new Thickness
            {
                Top = Context.FromPixels(visibleRect.Top),
                Bottom = Context.FromPixels(decoreHeight - visibleRect.Bottom),
                Right = Context.FromPixels(decoreWidht - visibleRect.Right),
                Left = Context.FromPixels(visibleRect.Left)
            };

            return result;
        }

        #endregion

        #region Life Cycle Methods

        protected override void OnAttachedToWindow()
        {
            Context.HideKeyboard(((Activity) Context).Window.DecorView);
            base.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(0), () =>
            {
                Popup.Context.HideKeyboard(((Activity) Popup.Context).Window.DecorView);
                return false;
            });
            base.OnDetachedFromWindow();
        }

        protected override void OnWindowVisibilityChanged(ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);

            // It is needed because a size of popup has not updated on Android 7+. See #209
            if (visibility == ViewStates.Visible)
                RequestLayout();
        }

        #endregion

        #region Touch Methods

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                _downTime = DateTime.UtcNow;
                _downPosition = new Point(e.RawX, e.RawY);
            }
            if (e.Action != MotionEventActions.Up)
                return base.DispatchTouchEvent(e);
            View currentFocus1 = ((Activity)Context).CurrentFocus;
            bool flag = base.DispatchTouchEvent(e);
            if (currentFocus1 is EditText)
            {
                View currentFocus2 = ((Activity)Context).CurrentFocus;
                if (currentFocus1 == currentFocus2 && _downPosition.Distance(new Point(e.RawX, e.RawY)) <= Context.ToPixels(20.0) && !(DateTime.UtcNow - _downTime > TimeSpan.FromMilliseconds(200.0)))
                {
                    int[] location = new int[2];
                    currentFocus1.GetLocationOnScreen(location);
                    float num1 = e.RawX + currentFocus1.Left - location[0];
                    float num2 = e.RawY + currentFocus1.Top - location[1];
                    if (!new Rectangle(currentFocus1.Left, currentFocus1.Top, currentFocus1.Width, currentFocus1.Height).Contains(num1, num2))
                    {
                        Context.HideKeyboard(currentFocus1);
                        RequestFocus();
                        currentFocus1.ClearFocus();
                    }
                }
            }
            return flag;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (_disposed)
                return false;

            var baseValue = base.OnTouchEvent(e);

            _gestureDetector.OnTouchEvent(e);

            if(CurrentElement != null && CurrentElement.BackgroundInputTransparent)
            {
                if (ChildCount > 0 && !IsInRegion(e.RawX, e.RawY, GetChildAt(0)) || ChildCount == 0)
                    return false;
            }

            return baseValue;
        }

        private void OnBackgroundClick(object sender, MotionEvent e)
        {
            if (ChildCount == 0)
                return;

            var isInRegion = IsInRegion(e.RawX, e.RawY, GetChildAt(0));

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
}