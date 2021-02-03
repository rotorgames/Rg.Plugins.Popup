using System;
using System.Runtime.Serialization;

namespace Rg.Plugins.Popup.Exceptions
{
    public class RGPopupStackInvalidException : Exception
    {
        public RGPopupStackInvalidException()
        {
        }

        public RGPopupStackInvalidException(string message) : base(message)
        {
        }

        public RGPopupStackInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RGPopupStackInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
