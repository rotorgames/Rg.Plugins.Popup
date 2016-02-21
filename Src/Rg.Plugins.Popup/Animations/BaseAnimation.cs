using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations
{
    internal class BaseAnimation : IPopupAnimation
    {
        public uint Time { get; protected set; } = 200;
        public Easing EasingIn { get; protected set; } = Easing.Linear;
        public Easing EasingOut { get; protected set; } = Easing.Linear;
        
        public virtual void Preparing(View content, PopupPage page)
        {
            
        }

        public virtual void Disposing(View content, PopupPage page)
        {
            
        }

        public virtual Task Appearing(View content, PopupPage page)
        {
            return null;
        }

        public virtual Task Disappearing(View content, PopupPage page)
        {
            return null;
        }

        protected int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }

        protected int GetLeftOffset(View content, Page page)
        {
            return (int)(content.Width + page.Width) / 2;
        }
    }
}
