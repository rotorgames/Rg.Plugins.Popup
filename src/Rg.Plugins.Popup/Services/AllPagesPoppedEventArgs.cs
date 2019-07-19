using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Rg.Plugins.Popup.Services
{
    public class AllPagesPoppedEventArgs : EventArgs
    {
        public IEnumerable<Page> PoppedPages { get; }

        public AllPagesPoppedEventArgs(IEnumerable<Page> poppedPages)
            => PoppedPages = poppedPages ?? throw new ArgumentNullException(nameof(poppedPages));
    }
}