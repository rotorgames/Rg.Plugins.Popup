using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using Rg.Plugins.Popup.Pages;
using Microsoft.Maui.Essentials;
using AsyncAwaitBestPractices;
using Rg.Plugins.Popup.Services;
using AsyncAwaitBestPractices.MVVM;
using Button = Microsoft.Maui.Controls.Button;
using ScrollView = Microsoft.Maui.Controls.ScrollView;

namespace SampleMaui.CSharpMarkup
{
    public partial class MainPage : ContentPage
    {
        protected void BuildContent()
        {
            BackgroundColor = Color.FromRgb(255, 255, 255);
            Title = "Popup Demo";
            Content = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = GenerateMainPageStackLayout()
            };
        }

        private StackLayout GenerateMainPageStackLayout()
        {
            var mainStackLayout = new StackLayout
            {
                Spacing = 20,
                Margin = new Microsoft.Maui.Thickness(10, 15)
            };
            mainStackLayout.Add(GeneratePopupButton("Open Popup", GenerateSimpleCommandForPopup<LoginPage>()));
            return mainStackLayout;
        }

        private static Button GeneratePopupButton(string buttonText, AsyncCommand buttonCommand)
        {
            return new Button
            {
                Text = buttonText,
                BackgroundColor = Color.FromHex("#FF7DBBE6"),
                TextColor = Color.FromRgb(255, 255, 255),
                Command = buttonCommand
            };
        }

        private static AsyncCommand GenerateSimpleCommandForPopup<TPopupPage>() where TPopupPage : PopupPage, new()
        {
            return new AsyncCommand(async () =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        var page = new TPopupPage();
                        await PopupNavigation.Instance.PushAsync(page);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
            });
        }
        

        
    }
}
