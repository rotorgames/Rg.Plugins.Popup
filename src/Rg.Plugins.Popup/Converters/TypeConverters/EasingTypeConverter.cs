using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Converters.TypeConverters
{
    public class EasingTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                FieldInfo fieldInfo = typeof(Easing).GetRuntimeFields()?.FirstOrDefault((fi =>
                {
                    if (fi.IsStatic)
                        return fi.Name == value;
                    return false;
                }));
                if (fieldInfo != null)
                    return (Easing)fieldInfo.GetValue(null);
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Easing)}");
        }
    }
}
