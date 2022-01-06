
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Pages;
using ScrollView = Microsoft.Maui.Controls.ScrollView;

namespace SampleMaui.CSharpMarkup
{
    public partial class LoginPage : PopupPage
    {
        public Frame FrameContainer { get; set; }
        public Image DotNetBotImage { get; set; }

        public Entry UsernameEntry { get; set; }
        public Entry PasswordEntry { get; set; }
        public Microsoft.Maui.Controls.Button LoginButton { get; set; }
        protected void BuildContent()
        {
            try
            {

                this.HeightRequest = 300;
                this.WidthRequest = 300;
                this.Animation = new ScaleAnimation
                {
                    DurationIn = 700,
                    EasingIn = Microsoft.Maui.Easing.BounceOut,
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Center,
                    ScaleIn = 1,
                    ScaleOut = 0.7
                };
                this.Content = new ScrollView
                {
                    WidthRequest = 300,
                    HeightRequest = 300,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Content = GenerateLoginView()
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Frame GenerateLoginView()
        {
            FrameContainer = new Frame
            {
                Margin = new Microsoft.Maui.Thickness(15),
                BackgroundColor = Microsoft.Maui.Graphics.Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                Content = GenerateFrameContainerContent()
            };
            return FrameContainer;
        }
        private StackLayout GenerateFrameContainerContent()
        {
            var frameContainerContent = new StackLayout
            {
                Padding = new Microsoft.Maui.Thickness(10, 5)
            };

            DotNetBotImage = new Image
            {
                Margin = new Microsoft.Maui.Thickness(10),
                BackgroundColor = Microsoft.Maui.Graphics.Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("dotnet_bot.svg")
            };

            UsernameEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.Center,
                Placeholder = "Username",
                PlaceholderColor = Microsoft.Maui.Graphics.Color.FromHex("#FF9CDAF1"),
                TextColor = Microsoft.Maui.Graphics.Color.FromHex("#FF7DBBE6")
            };

            PasswordEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.Center,
                IsPassword = true,
                Placeholder = "Password",
                PlaceholderColor = Microsoft.Maui.Graphics.Color.FromHex("#FF9CDAF1"),
                TextColor = Microsoft.Maui.Graphics.Color.FromHex("#FF7DBBE6")
            };


            frameContainerContent.Add(DotNetBotImage);
            frameContainerContent.Add(UsernameEntry);
            frameContainerContent.Add(PasswordEntry);

            return frameContainerContent;
        }
    }
}
