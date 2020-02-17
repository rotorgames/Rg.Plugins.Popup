using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.GTK.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.GTK.Renderers;
using Rg.Plugins.Popup.GTK.Renderers;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Rg.Plugins.Popup.GTK.Renderers
{
    class PopupPageRenderer : Xamarin.Forms.Platform.GTK.Renderers.PageRenderer
    {
        internal PopupControl Container { get; private set; }

        private PopupPage CurrentElement => (PopupPage)Element;

        [Preserve]
        public PopupPageRenderer()
        {

        }

        internal void Prepare(PopupControl container)
        {
            Container = container;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            Container.SizePopup();
        }
        internal void Destroy()
        {
            Container.Close();
            Container = null;
        }
    }
}
