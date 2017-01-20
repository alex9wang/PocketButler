using PocketButler.Controls;
using PocketButler.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PocketButler
{
	public class MenuPage : BasePage
    {
        #region PUBLIC MEMBERS
        public ListView Menu { get; set; }
        #endregion

        #region PRIVATE MEMBERS
        readonly List<OptionItem> OptionItems = new List<OptionItem>();
        #endregion

		public MenuPage () : base(true)
		{
			Title = "Menu Page";
            
			BackgroundImage = "right_line.png";

            InitializeListItems();

            Menu = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = OptionItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
                RowHeight = 55,
            };

            var cell = new DataTemplate(typeof(CustomImageCell));
            cell.SetBinding(CustomImageCell.TextProperty, "Text");
			cell.SetBinding(CustomImageCell.DetailProperty, "Detail");
            cell.SetBinding(CustomImageCell.ImageSourceProperty, "Icon");
            cell.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);

            Menu.ItemTemplate = cell;

            Content = new StackLayout
            {
				Padding = 0,
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
					Menu,
					new StackLayout{
						Padding = 0,
						Spacing = 0,
						HorizontalOptions = LayoutOptions.End,
						VerticalOptions = LayoutOptions.FillAndExpand,
						WidthRequest = 1,
						BackgroundColor = Color.FromRgb(75, 75, 75),
					}
				}
            };
		}

        private void InitializeListItems()
        {
			if (Globals.Config.IsGuestMode == false) {
				OptionItems.Add (new OptionItem (0, "Browse Venues", "", "sidebar_homepage.png"));
				OptionItems.Add (new OptionItem (1, "My Orders", "", "sidebar_orders.png"));
				OptionItems.Add (new OptionItem (2, "Current Venue", "Venue not selected", "sidebar_venue.png"));
				OptionItems.Add (new OptionItem (3, "My Favourites", "", "sidebar_myfavourites.png"));
				OptionItems.Add (new OptionItem (4, "Settings", "", "sidebar_settings.png"));
				OptionItems.Add (new OptionItem (5, "About Us", "", "sidebar_aboutus.png"));
				OptionItems.Add (new OptionItem (6, "Log Out", "", "sidebar_logout.png"));
			} else {
				OptionItems.Add (new OptionItem (0, "Sign Up!", "", "sidebar_signup.png"));
				OptionItems.Add (new OptionItem (1, "Browse Venues", "", "sidebar_homepage.png"));
				OptionItems.Add (new OptionItem (2, "Current Venue", "Venue not selected", "sidebar_venue.png"));
				OptionItems.Add (new OptionItem (3, "About Us", "", "sidebar_aboutus.png"));
			}
        }

		public void ChangeCurrentVenueLabel(bool isSelected)
		{
			String venueInfo = "";
			if (isSelected == false || Globals.Config.CurrentVenue == null)
				venueInfo = "Venue not selected";
			else
				venueInfo = Globals.Config.CurrentVenue.name;

			OptionItems [2].Detail = venueInfo;
			Menu.ItemsSource = OptionItems;
		}
	}
}

