using System;
using System.Runtime.Serialization;

namespace Rg.Plugins.Popup.Exceptions
{
    public class RGPageInvalidException : Exception
    {
        public RGPageInvalidException()
        {
        }

        public RGPageInvalidException(string message) : base(message)
        {
        }

        public RGPageInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RGPageInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
