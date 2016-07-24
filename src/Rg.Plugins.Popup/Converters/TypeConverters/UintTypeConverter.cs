using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Converters.TypeConverters
{
    public class UintTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            try
            {
                return Convert.ToUInt32(value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Cannot convert {value} into {typeof(uint)}");
            }
        }
    }
}
