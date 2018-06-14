using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Demo.Pages
{
    public partial class SystemOffsetPage : PopupPage
    {
        public SystemOffsetPage()
        {
            var bc = new VM();
            BindingContext = bc;
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromMilliseconds(2000), () =>
            {
                bc.Padding = new Thickness(10,10,10,10);
                return false;
            });
            Device.StartTimer(TimeSpan.FromMilliseconds(4000), () =>
            {
                bc.IsSystemPadding = false;
                return false;
            });
            Device.StartTimer(TimeSpan.FromMilliseconds(6000), () =>
            {
                Padding = new Thickness();
                return false;
            });
            Device.StartTimer(TimeSpan.FromMilliseconds(8000), () =>
            {
                HasSystemPadding = true;
                return false;
            });
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        private Thickness _padding;
        private bool _isSystemPadding = true;
        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                OnPropertyChanged("Padding");
            }
        }

        public bool IsSystemPadding
        {
            get { return _isSystemPadding; }
            set
            {
                _isSystemPadding = value;
                OnPropertyChanged("IsSystemPadding");
            }
        }
    }
}
