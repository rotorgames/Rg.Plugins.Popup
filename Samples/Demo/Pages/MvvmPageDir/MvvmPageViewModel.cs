using System.Windows.Input;
using Xamarin.Forms;

namespace Demo.Pages
{
    public class MvvmPageViewModel
    {
        public ICommand BackgroundClickedCommand => new Command(BackgroundClickedCommandExecute);

        private void BackgroundClickedCommandExecute(object parameter)
        {
            var label = (Label) parameter;
            label.Text = "Great, it works!";
        }
    }
}
