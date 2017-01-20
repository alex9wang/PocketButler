using System;
using Xamarin.Forms;
using PocketButler.Controls;
using PocketButler.Services;
using PocketButler.Globals;

namespace PocketButler
{
	public class LoginPage : BasePage
	{
		protected AbsoluteLayout MainLayout { get; set; }

		DarkIceImage LogoImage;
		DILabel TopLabel;
		CustomLabel BottomHintLabel;

		Button GuestButton;
		//		FacebookLoginButton SignFaceBookImage;
		DarkIceImage SignFaceBookImage;

		Button EmailBackButton;
		DarkIceImage EmailImage;
		CustomEntry EmailEntry;

		DarkIceImage CheckOptImage;
		DILabel NextLabel;

		bool isRemember;
		bool isFromFacebook = false;

		new DarkIceImage BackgroundImage;

		String FirstName = "";
		String LastName = "";
		DateTime? DOB = null;
		String FacebookId = null;

		public LoginPage (string email = "", string firstName = "", string lastName = "", DateTime? dob = null, string facebookId = "")
		{
			isFromFacebook = true;
			FirstName = firstName;
			LastName = lastName;
			DOB = dob;
			FacebookId = facebookId;

			App.IsWindowAdjustResize = true;
			HideNavigationBar ();

			BindingContext = new LoginViewModel ();

			MainLayout = new AbsoluteLayout {
				VerticalOptions = LayoutOptions.Fill,
				BackgroundColor = Color.Transparent
			};

			// Read Settings
			isRemember = (Utils.LoadDataFromSettings ("remember_password") == "1");
			IsRootPageBackAction = true;
			BuildUI ();

			EmailEntry.Text = email;
			NextButton_Clicked ();
		}

		public LoginPage()
		{
			App.IsWindowAdjustResize = true;
			HideNavigationBar ();

			BindingContext = new LoginViewModel ();

			MainLayout = new AbsoluteLayout {
				VerticalOptions = LayoutOptions.Fill,
				BackgroundColor = Color.Transparent
			};

			// Read Settings
			isRemember = (Utils.LoadDataFromSettings ("remember_password") == "1");
			IsRootPageBackAction = true;
			BuildUI ();
		}

		protected override void OnAppearing ()
		{
			//App.IsWindowAdjustResize = true;
			base.OnAppearing ();
		}

		#region PRIVATE METHODS
		private void BuildUI ()
		{
			BackgroundImage = new DarkIceImage {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile ("welcome_background.png"),
				Aspect = Aspect.Fill,
			};

			AbsoluteLayout.SetLayoutFlags (BackgroundImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (BackgroundImage, new Rectangle (0, 0, 1, 1));
			MainLayout.Children.Add (BackgroundImage);

			LogoImage = new DarkIceImage {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile ("welcome_logo.png"),
				Aspect = Aspect.Fill
			};

			AbsoluteLayout.SetLayoutFlags (LogoImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (LogoImage, new Rectangle (0.5, 0.22, 0.5, 0.07));
			MainLayout.Children.Add (LogoImage);

			TopLabel = new DILabel {
				Text = "Enter your email address to sign in or create a new account",
				TextColor = Color.White,
				XAlign = TextAlignment.Center
			};
			TopLabel.SetBoldFont (NamedSize.Medium);

			AbsoluteLayout.SetLayoutFlags (TopLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (TopLabel, new Rectangle (0.5, 0.39, 0.8, 0.09));
			MainLayout.Children.Add (TopLabel);

			BottomHintLabel = new CustomLabel {
				Text = "Learn about Pocket Butler",
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize (NamedSize.Small),
				IsUnderline = true
			};

			BottomHintLabel.Tapped += Show_LearnAboutPage;

			AbsoluteLayout.SetLayoutFlags (BottomHintLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (BottomHintLabel, new Rectangle (0.5, 0.93, 0.8, 0.03));
			MainLayout.Children.Add (BottomHintLabel);

			GuestButton = new Button {
				Text = "Continue as Guest",
				BackgroundColor = Color.FromRgb (171, 146, 91),
				TextColor = Color.FromRgb (54, 54, 54),
				Font = Font.SystemFontOfSize (NamedSize.Medium)
			};
			GuestButton.SetBinding (Button.CommandProperty, LoginViewModel.GuestCommandPropertyName);

			AbsoluteLayout.SetLayoutFlags (GuestButton, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (GuestButton, new Rectangle (0.5, 0.84, 0.93, 0.08));
			MainLayout.Children.Add (GuestButton);

			//SignFaceBookButton = new Button{ Text = "Sign in with Facebook", BackgroundColor = Color.Transparent, TextColor = Color.White, Font = Font.SystemFontOfSize(NamedSize.Medium, FontAttributes.Bold), BorderColor = Color.FromRgb(145, 145, 145) };
//			SignFaceBookImage = new FacebookLoginButton ();
			SignFaceBookImage = new DarkIceImage {
				Source = ImageSource.FromFile ("welcome_btn_facebook.png"),
				Aspect = Aspect.Fill,
			};


			AbsoluteLayout.SetLayoutFlags (SignFaceBookImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (SignFaceBookImage, new Rectangle (0.5, 0.73, 0.93, 0.08));
			MainLayout.Children.Add (SignFaceBookImage);
			SignFaceBookImage.Tapped += SignInFaceBook_Clicked;

			EmailBackButton = new Button {
				Text = "",
				BackgroundColor = Color.Transparent,
				BorderColor = Color.FromRgb (145, 145, 145),
				BorderRadius = 1,
				BorderWidth = 1
			};
			AbsoluteLayout.SetLayoutFlags (EmailBackButton, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (EmailBackButton, new Rectangle (0.5, 0.5, 1, 0.08));
			MainLayout.Children.Add (EmailBackButton);

			EmailImage = new DarkIceImage {
				Source = ImageSource.FromFile ("login_email.png"),
				Aspect = Aspect.AspectFit
			};
			AbsoluteLayout.SetLayoutFlags (EmailImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (EmailImage, new Rectangle (0.04, 0.5, 0.08, 0.07));
			MainLayout.Children.Add (EmailImage);

			EmailEntry = new CustomEntry { 
				Placeholder = "Email",
				TextColor = Color.White,
				BackgroundColor = Color.Transparent,
				HasBorder = false,
				PlaceholderTextColor = Color.Gray,
				Text = Utils.LoadDataFromSettings("user_email")
			};
			AbsoluteLayout.SetLayoutFlags (EmailEntry, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (EmailEntry, new Rectangle (1, 0.5, 0.86, 0.07));
			MainLayout.Children.Add (EmailEntry);

			CheckOptImage = new DarkIceImage {
				Source = GetOptImageWithStatus (isRemember),
				Aspect = Aspect.AspectFit
			};
			CheckOptImage.Tapped += OnRememberClicked;
			AbsoluteLayout.SetLayoutFlags (CheckOptImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (CheckOptImage, new Rectangle (0.04, 0.6, 0.07, 0.07));
			MainLayout.Children.Add (CheckOptImage);

			DILabel EmailLabel = new DILabel {
				Text = "Remember Email",
				Font = Font.SystemFontOfSize (NamedSize.Medium),
				TextColor = Color.FromRgb (170, 170, 170),
				YAlign = TextAlignment.Center
			};
			EmailLabel.Tapped += OnRememberClicked;
			AbsoluteLayout.SetLayoutFlags (EmailLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (EmailLabel, new Rectangle (0.4, 0.6, 0.7, 0.07));
			MainLayout.Children.Add (EmailLabel);

			NextLabel = new DILabel {
				Text = "Next",
				TextColor = Color.FromRgb (171, 146, 91),
				Font = Font.SystemFontOfSize (NamedSize.Medium),
				BackgroundColor = Color.Transparent,
				YAlign = TextAlignment.Center,
				XAlign = TextAlignment.End,
			};
			AbsoluteLayout.SetLayoutFlags (NextLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (NextLabel, new Rectangle (0.95, 0.6, 0.15, 0.07));
			MainLayout.Children.Add (NextLabel);

			NextLabel.Tapped += NextButton_Clicked;

			Content = new ScrollView() { Content = MainLayout };
		}

		private String GetOptImageWithStatus (bool isSelected)
		{
			if (isSelected == true)
				return "welcome_remember_email_checked.png";
			else
				return "welcome_remember_email_empty.png";
		}

		#endregion

		#region BUTTON CLICKS

		private void OnRememberClicked ()
		{
			isRemember = !isRemember;

			CheckOptImage.Source = ImageSource.FromFile (GetOptImageWithStatus (isRemember));
		}

		async void NextButton_Clicked ()
		{
			ShowLoading ();

			var device_token = Utils.LoadDataFromSettings ("device_token");

			Utils.MixPanel_Track("EmailEntered", "{UserEmail:" + EmailEntry.Text + "}");

			// Check if user is registered or not
			var response = await LoginServices.CheckUserName (EmailEntry.Text, "", device_token, "Android");

			HideLoading ();

			Globals.Config.IsGuestMode = false;
			Globals.Config.USER_EMAIL = EmailEntry.Text;

			Utils.SaveDataToSettings ("remember_password", (isRemember) ? "1" : "0");
			Utils.SaveDataToSettings ("user_email", (isRemember) ? EmailEntry.Text : "");

			App.IsWindowAdjustResize = false;
			bool isRegistered = LoginServices.HasSuccessResult (response);
			if (isRegistered) {
				App._NavigationPage.PushAsync (new SigninPage (PageShowingEvent));
			}
			else {
				if (isFromFacebook) {
					Utils.MixPanel_Track("SignUpWithFacebook", "{$email:" + EmailEntry.Text + ", $first_name:" + FirstName + ", $last_name:" + LastName + "}");
					App._NavigationPage.PushAsync (new CreateAccountPage (PageShowingEvent, EmailEntry.Text, FirstName, LastName, DOB, FacebookId));
				}
				else
					App._NavigationPage.PushAsync (new CreateAccountPage (PageShowingEvent));
			}
		}

		void SignInFaceBook_Clicked()
		{
			//App._FacebookPage = new FacebookLoginPage ();
			//Navigation.PushAsync (App._FacebookPage);
			App.PageLoaderManager.FacebookLogin ();
		}
		#endregion

	}
}

