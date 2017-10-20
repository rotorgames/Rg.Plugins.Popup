using System;
using Android.Views;

namespace Rg.Plugins.Popup.Droid.Gestures
{
    internal class RgGestureDetectorListener : Java.Lang.Object, GestureDetector.IOnGestureListener
    {
        public event EventHandler Clicked;

        public bool OnDown(MotionEvent e)
        {
            // Ignored
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            // Ignored
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
            // Ignored
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            // Ignored
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
            // Ignored
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            return true;
        }
    }
}