using System;
using System.Collections.Generic;
using System.Text;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.IOS.Impl;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenHelperIos))]
namespace Rg.Plugins.Popup.IOS.Impl
{
    [Preserve(AllMembers = true)]
    class ScreenHelperIos : IScreenHelper
    {
        public Rectangle ScreenSize
        {
            get
            {
                var screen = UIScreen.MainScreen.Bounds;
                var size = new Rectangle
                {
                    Top = screen.Top,
                    Bottom = screen.Bottom,
                    Left = screen.Left,
                    Right = screen.Right,
                    Width = screen.Width,
                    Height = screen.Height
                };
                return size;
            }
        }

        public Thickness ScreenOffsets
        {
            get
            {
                var height = UIApplication.SharedApplication.StatusBarFrame.Size.Height;
                var result = new Thickness
                {
                    Top = height
                };
                return result;
            }
        }
    }
}
