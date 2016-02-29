using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rg.Plugins.Popup
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = true)]
    public sealed class PreserveAttribute : Attribute
    {
        public bool AllMembers;
        public bool Conditional;

        /// <summary>
        /// Instruct the MonoTouch linker to preserve the decorated code
        /// </summary>
        /// 
        /// <remarks>
        /// By default the linker, when enabled, will remove all the code that is not directly used by the application.
        /// </remarks>
        public PreserveAttribute()
        {
        }

        public PreserveAttribute(Type type)
        {
        }
    }
}
