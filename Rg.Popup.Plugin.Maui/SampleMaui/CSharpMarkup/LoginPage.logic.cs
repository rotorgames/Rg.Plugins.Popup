
using Rg.Plugins.Popup.Pages;

namespace SampleMaui.CSharpMarkup
{
    public partial class LoginPage : PopupPage
    {
        public LoginPage()
        {
            BuildContent();

        }

        protected override void OnAppearingAnimationBegin()
        {
            //base.OnAppearingAnimationBegin();

            //FrameContainer.HeightRequest = 1;

            if (!IsAnimationEnabled)
            {
                //CloseImage.Rotation = 0;
                //CloseImage.Scale = 1;
                //CloseImage.Opacity = 1;

                //LoginButton.Scale = 1;
                //LoginButton.Opacity = 1;

                //UsernameEntry.TranslationX = PasswordEntry.TranslationX = 0;
                //UsernameEntry.Opacity = PasswordEntry.Opacity = 1;

                //return;
            }

            //CloseImage.Rotation = 30;
            //CloseImage.Scale = 0.3;
            // CloseImage.Opacity = 0;

            //LoginButton.Scale = 0.3;
            //LoginButton.Opacity = 0;

            //UsernameEntry.TranslationX = PasswordEntry.TranslationX = -10;
            //UsernameEntry.Opacity = PasswordEntry.Opacity = 0;
        }

        protected override async Task OnAppearingAnimationEndAsync()
        {
            if (!IsAnimationEnabled)
            {
                return;
            }

            // var translateLength = 400u;
            /*
             await Task.WhenAll(
                 UsernameEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                 UsernameEntry.FadeTo(1),
                 (new Func<Task>(async () =>
                 {
                     await Task.Delay(200);
                     await Task.WhenAll(
                         PasswordEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                         PasswordEntry.FadeTo(1));

                 }))());
            */
            //await Task.WhenAll(
            //CloseImage.FadeTo(1),
            //CloseImage.ScaleTo(1, easing: Easing.SpringOut),
            //CloseImage.RotateTo(0),
            //LoginButton.ScaleTo(1),
            //LoginButton.FadeTo(1));
        }

        /*
        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            await Task.WhenAll(
                UsernameEntry.FadeTo(0),
                PasswordEntry.FadeTo(0));//,
                //LoginButton.FadeTo(0));

            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            },
            start: currentHeight,
            end: 170,
            finished: async (d, b) =>
            {
                await Task.Delay(300);
                taskSource.TrySetResult(true);
            });

            await taskSource.Task;
        }
        */

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
    }
}
