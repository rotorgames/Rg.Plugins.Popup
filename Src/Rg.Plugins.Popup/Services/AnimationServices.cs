using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;

namespace Rg.Plugins.Popup.Services
{
    internal static class AnimationServices
    {
        public static IPopupAnimation GeAnimation(AnimationsName animation)
        {
            switch (animation)
            {
                case AnimationsName.Others:
                    return null;
                case AnimationsName.Fade:
                    return new FadeAnimation();
                case AnimationsName.PopIn:
                    return new PopInAnimation();
            }

            return null;
        }
    }
}
