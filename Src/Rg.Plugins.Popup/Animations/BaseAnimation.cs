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
        public uint Time { get; protected set; } = 300;
        public Easing EasingIn { get; protected set; } = Easing.Linear;
        public Easing EasingOut { get; protected set; } = Easing.Linear;
        
        public virtual void Begin(View content, PopupPage page)
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
    }
}
