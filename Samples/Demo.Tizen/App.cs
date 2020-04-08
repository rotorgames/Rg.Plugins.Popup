using Xamarin.Forms;

namespace Demo.Tizen
{
	class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			LoadApplication(new App());
		}

		static void Main(string[] args)
		{
			var app = new Program();
            Rg.Plugins.Popup.Tizen.Popup.Init();
			Forms.Init(app, true);
			app.Run(args);
		}
	}
}
