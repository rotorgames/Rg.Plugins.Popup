# Popup Page Plugin for Xamarin Forms [![Build status](https://ci.appveyor.com/api/projects/status/faktrrb31efbqq3x/branch/master?svg=true)](https://ci.appveyor.com/project/Kirill/rg-plugins-popup/branch/master)
The plugin allows you to open any page as a popup.

Nuget: https://www.nuget.org/packages/Rg.Plugins.Popup/

![Landing](img/popup-landing.png)

## Support platforms

- [x] Android
- [x] iOS
- [x] Windows Phone
- [x] UWP

## Override Methods PopupPage

* OnAppearing
* OnDisappearing
* OnBackButtonPressed
* OnAppearingAnimationEnd
* OnDisappearingAnimationBegin
* OnBackgroundClicked

## Events

* BackgroundClicked: Called when clicked on background

## Animations

#### FadeAnimation

* DurationIn (uint)
* DurationOut (uint)
* EasingIn (Easing)
* EasingOut (Easing)
* HasBackgroundAnimation (bool)

#### MoveAnimation

* PositionIn (MoveAnimationOptions)
* PositionOut (MoveAnimationOptions)
* DurationIn (uint)
* DurationOut (uint)
* EasingIn (Easing)
* EasingOut (Easing)
* HasBackgroundAnimation (bool)

#### ScaleAnimation

* ScaleIn (double)
* ScaleOut (double)
* PositionIn (MoveAnimationOptions)
* PositionOut (MoveAnimationOptions)
* DurationIn (uint)
* DurationOut (uint)
* EasingIn (Easing)
* EasingOut (Easing)
* HasBackgroundAnimation (bool)


## Initialize

Initialization is not required for Android, iOS, WP.

UWP required add assemblies in Xamarin.Forms.Forms.Init method if you use .NET Native compile:

#### UWP Example
```csharp
// In App.xaml.cs
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
    ...

    // Initialization is required due to an error when compiling in release mode.
    // Details: https://developer.xamarin.com/guides/xamarin-forms/platform-features/windows/installation/universal/#Troubleshooting
    
    Xamarin.Forms.Forms.Init(e, Rg.Plugins.Popup.Windows.Popup.GetExtraAssemblies());

	// or if you have other renderers and DependencyService implementations

	var assemblies = new List<Assembly>
    {
	   typeof(YourRenderer).GetTypeInfo().Assembly
	   typeof(YourServiceImplementation).GetTypeInfo().Assembly
    };

	Xamarin.Forms.Forms.Init(e, Rg.Plugins.Popup.Windows.Popup.GetExtraAssemblies(assemblies));

    ...
}
```

## PopupPage Properties

* IsAnimating
* Animation
* BackgroundColor: Hex #80FF5C5C where #80 opacity [Range](http://stackoverflow.com/questions/5445085/understanding-colors-in-android-6-characters/11019879#11019879)
* CloseWhenBackgroundIsClicked: Close pop-up when click on the background
* HasSystemPadding: Enabled/Disabled system padding offset (Only for Content not for Background)
	
	![Android](/icons/system-padding-droid.png) ![Android](/icons/system-padding-ios.png)
* SystemPadding: (ReadOnly) Thickness

## How To Use

```csharp 

// Use these methods in PopupNavigation globally or Navigation in your pages

// Open new PopupPage
Task PushAsync(PopupPage page, bool animate = true) // Navigation.PushPopupAsync

// Hide last PopupPage
Task PopAsync(bool animate = true) // Navigation.PopPopupAsync

// Hide all PopupPage with animations
Task PopAllAsync(bool animate = true) // Navigation.PopAllPopupAsync

// Remove one popup page in stack
Task RemovePageAsync(PopupPage page, bool animate = true) // Navigation.RemovePopupPageAsync
```

```xml
<?xml version="1.0" encoding="utf-8" ?>
<pages:PopupPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:pages="clr-namespace:Rg.Plugins.Popup.Pages;assembly=Rg.Plugins.Popup"
             xmlns:animations="clr-namespace:Rg.Plugins.Popup.Animations;assembly=Rg.Plugins.Popup"
             x:Class="Demo.Pages.MyPopupPage">
  <!--Animations use example-->
  <pages:PopupPage.Animation>
    <animations:ScaleAnimation 
      PositionIn="Center"
      PositionOut="Center"
      ScaleIn="1.2"
      ScaleOut="0.8"
      DurationIn="400"
      DurationOut="300"
      EasingIn="SinOut"
      EasingOut="SinIn"
      HasBackgroundAnimation="True"/>
  </pages:PopupPage.Animation>
  <!-- Content -->
</pages:PopupPage>
```
```csharp
public partial class MyPopupPage : PopupPage
    {
        public SecondPopupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        
		// Method for animation child in PopupPage
		// Invoced after custom animation end
        protected virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

		// Method for animation child in PopupPage
		// Invoked before custom animation begin
        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);;
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true; 
        }

		// Invoced when background is clicked
		protected override bool OnBackgroundClicked()
        {
			// Return default value - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }
    }
    
    // Main Page
    
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        
        // Button Click
        private async void OnOpenPupup(object sender, EventArgs e)
        {
            var page = new MyPopupPage();
            
            await Navigation.PushPopupAsync(page);
            // or
            await PopupNavigation.PushAsync(page);
        }
    }
```

## User Animation

```csharp
    // User animation
    class UserAnimation : IPopupAnimation
    {
        // Call Before OnAppering
        public void Preparing(View content, PopupPage page)
        {
            // Preparing content and page
            content.Opacity = 0;
        }

		// Call After OnDisappering
        public void Disposing(View content, PopupPage page)
        {
			// Dispose Unmanaged Code
        }
        
        // Call After OnAppering
        public async Task Appearing(View content, PopupPage page)
        {
            // Show animation
            await content.FadeTo(1);
        }
        
        // Call Before OnDisappering
        public async Task Disappearing(View content, PopupPage page)
        {
            // Hide animation
            await content.FadeTo(0);
        }
    }
    
    // Popup Page
    public partial class UserPopupPage : PopupPage
    {
        public SecondPopupPage()
        {
            InitializeComponent();
            Animation = new UserAnimation();
        }
    }
```

Or in xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<pages:PopupPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:pages="clr-namespace:Rg.Plugins.Popup.Pages;assembly=Rg.Plugins.Popup"
             xmlns:animations="clr-namespace:Demo.Animations;assembly=Demo"
             x:Class="Demo.Pages.UserAnimationPage">
  <pages:PopupPage.Animation>
    <animations:UserAnimation/>
  </pages:PopupPage.Animation>
  ...
</pages:PopupPage>
```

## Thanks

* [xam-forms-transparent-modal](https://github.com/gaborv/xam-forms-transparent-modal)

## License

The MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
