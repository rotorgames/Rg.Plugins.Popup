
using Microsoft.Maui;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Rg.Plugins.Popup.Converters.TypeConverters
{
    public class EasingTypeConverter : TypeConverter
    {
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value != null)
            {
                FieldInfo? fieldInfo = typeof(Easing).GetRuntimeFields()?.FirstOrDefault(fi =>
                {
                    if (fi.IsStatic)
                        return fi.Name == value.ToString();
                    return false;
                });
                if (fieldInfo != null)
                    return (Easing?)fieldInfo.GetValue(null);
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Easing)}");
        }
    }
}
