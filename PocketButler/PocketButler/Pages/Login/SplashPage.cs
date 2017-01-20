using System;
using Xamarin.Forms;

namespace PocketButler
{
	public class SplashPage : BasePage
	{
		public SplashPage ()
		{
			BackgroundColor = Color.White;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Utils.Sleep (3000);

			var homePage = new LoginPage ();

			Navigation.PushModalAsync (homePage);            
		}
	}
}

