using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Rg.Plugins.Popup.GTK.Controls
{
    class PopupControl : FormsWindow
    {
        public PopupControl()
        {
            Gtk.Application.Init();
            Forms.Init();
            BuildPopupControl();
        }

        private void BuildPopupControl()
        {
            TypeHint = Gdk.WindowTypeHint.Normal;
            Modal = true;
            BorderWidth = 0;
            SizePopup();
            Resizable = false;
            AllowGrow = false;
            Decorated = false;
            DestroyWithParent = true;
            SkipPagerHint = true;
            SkipTaskbarHint = true;
            AppPaintable = true;
        }
        public void SizePopup()
        {
            var windows = Gtk.Window.ListToplevels();
            this.TransientFor = windows.FirstOrDefault();
            WindowPosition = Gtk.WindowPosition.CenterOnParent;
        }

        public void Close()
        {
            Xamarin.Forms.Platform.GTK.Helpers.GrabHelper.RemoveGrab(this);
            Destroy();
        }
    }
}
