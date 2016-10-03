using System.Threading.Tasks;
using Rg.Plugins.Popup.Converters.TypeConverters;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Animations.Base
{
    public abstract class BaseAnimation : IPopupAnimation
    {
        private const uint DefaultDuration = 200;

        [TypeConverter(typeof (UintTypeConverter))]
        public uint DurationIn { get; set; } = DefaultDuration;

        [TypeConverter(typeof (UintTypeConverter))]
        public uint DurationOut { get; set; } = DefaultDuration;

        [TypeConverter(typeof(EasingTypeConverter))]
        public Easing EasingIn { get; set; } = Easing.Linear;

        [TypeConverter(typeof(EasingTypeConverter))]
        public Easing EasingOut { get; set; } = Easing.Linear;

        public abstract void Preparing(View content, PopupPage page);

        public abstract void Disposing(View content, PopupPage page);

        public abstract Task Appearing(View content, PopupPage page);

        public abstract Task Disappearing(View content, PopupPage page);

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
