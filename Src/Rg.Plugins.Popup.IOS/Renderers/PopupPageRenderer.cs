using System;
using System.Collections.Generic;
using System.Text;
using Rg.Plugins.Popup.IOS.Renderers;
using Rg.Plugins.Popup.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.IOS.Renderers
{
    class PopupPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            this.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
        }
    }
}
