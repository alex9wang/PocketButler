using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using PocketButler.Controls;

namespace PocketButler
{
    public class OrderPage : BasePage
    {
		#region PRIVATE MEMBERS
		protected AbsoluteLayout UILayout { get; set; }
		protected AbsoluteLayout MainLayout { get; set; }

		StackLayout PaymentItemsLayout { get; set; }
		CustomExpandableListView PaymentListView { get; set; }

		bool IsCustomizeRaised = false;
		#endregion

		public OrderPage(Action RefreshEvent)
        {
			BackAppearingEvent = RefreshEvent;

			Title = "Order Screen";

			UILayout = new AbsoluteLayout
			{
				VerticalOptions = LayoutOptions.Fill,
				BackgroundColor = Color.Transparent
			};

			MainLayout = new AbsoluteLayout
			{
				VerticalOptions = LayoutOptions.Fill,
				BackgroundColor = Color.Transparent
			};

			BuildUI();
			PageShowingEvent += OnPageShowing;
        }

		public override async void OnPageShowing()
		{
			base.OnPageShowing ();

			if (Globals.Config.CustomizeEventHandle.IsCustomizeUpdated == true) {
				List<ExpandableTableListItem> dataList = GetListPaymentItems();
				if (dataList == null || dataList.Count == 0) {
					if (BackAppearingEvent != null)
						BackAppearingEvent.Invoke ();
					await Navigation.PopAsync ();
				}
				else
					PaymentListView.ListData = dataList;

				Globals.Config.CustomizeEventHandle.IsCustomizeUpdated = false;
			}
		}
		#region PRIVATE METHODS
		private void BuildUI()
		{
			// Register Custom Navigation Bar
			var MenuImage = new DarkIceImage
			{
				Source = ImageSource.FromFile ("navbar_sidebar.png"),
				Aspect = Aspect.AspectFit,
				IsEnablePadding = true,
			};

			MakeCustomNavigationBar(UILayout, MenuImage, null, true);

			MenuImage.Tapped += MenuButton_Clicked;

			PaymentListView = new CustomExpandableListView { 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ListData = GetListPaymentItems(),
			};

			PaymentListView.DeleteDelegate = PaymentItem_Delete_Clicked;
			PaymentListView.EditDelegate = PaymentItem_Edit_Item_Clicked;

			AbsoluteLayout.SetLayoutFlags(PaymentListView, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(PaymentListView, new Rectangle(0, 0.05, 1, 0.65));
			MainLayout.Children.Add(PaymentListView);

			var TotalInfoBackButton = new Button {
				Text = "",
				BackgroundColor = Color.Transparent,
				BorderColor = Color.FromRgb (94, 94, 94),
				BorderRadius = 1,
				BorderWidth = 1
			};
			AbsoluteLayout.SetLayoutFlags(TotalInfoBackButton, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(TotalInfoBackButton, new Rectangle(0.5, 0.75, 1, 0.1));
			MainLayout.Children.Add(TotalInfoBackButton);

			var TotalLabel = new DILabel {
				Text = "Total",
				TextColor = Color.White,
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				YAlign = TextAlignment.Center,
			};
			AbsoluteLayout.SetLayoutFlags(TotalLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(TotalLabel, new Rectangle(0.1, 0.75, 0.5, 0.1));
			MainLayout.Children.Add(TotalLabel);

			var TotalPriceLabel = new DILabel {
				Text = "",
				TextColor = Color.White,
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				YAlign = TextAlignment.Center,
			};
			AbsoluteLayout.SetLayoutFlags(TotalPriceLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(TotalPriceLabel, new Rectangle(0.9, 0.75, 0.2, 0.1));
			MainLayout.Children.Add(TotalPriceLabel);

			TotalPriceLabel.BindingContext = Globals.Config.PaymentInfo;
			TotalPriceLabel.SetBinding (Label.TextProperty, "Price");

			var HintAlcholLabel = new DILabel {
				Text = "It is against the law to sell or supply alcohol to, or to obtain alcohol on behalf of, a person under the age of 18 years.",
				TextColor = Color.White,
				Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Italic),
				YAlign = TextAlignment.Center,
				Lines = 2,
				IsDefaultLabel = true,
				LineBreakMode = LineBreakMode.CharacterWrap,
			};

			AbsoluteLayout.SetLayoutFlags(HintAlcholLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(HintAlcholLabel, new Rectangle(0.5, 0.9, 0.9, 0.2));
			MainLayout.Children.Add(HintAlcholLabel);

			var CheckoutButton = new Button {
				Text = "Check Out",
				TextColor = Color.Black,
				BackgroundColor = Color.FromRgb(171, 146, 91),
			};
			AbsoluteLayout.SetLayoutFlags(CheckoutButton, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(CheckoutButton, new Rectangle(0.5, 0.98, 0.8, 0.08));
			MainLayout.Children.Add(CheckoutButton);
			CheckoutButton.Clicked += async (object sender, EventArgs e) => {

				if(PaymentListView.ListData == null || PaymentListView.ListData.Count == 0){
					await DisplayAlert ("Info", "No Order", "OK");
					return;
				}

				if (Globals.Config.CurrentVenue.settings.is_pickup_enabled.Equals("1") && Globals.Config.CurrentVenue.settings.is_tableservice_enabled.Equals("1"))
				{
					var action = await DisplayActionSheet ("Select a method", "Cancel", null, "Pick-up", "Table Service");
					if (action == "Pick-up")
					{
						Globals.Config.PaymentType = PocketButler.Utils.DeliveryType.DeliveryTypePickup;
						await Navigation.PushAsync(new ServiceTablePage(PageShowingEvent));
					}
					else if (action == "Table Service")
					{
						Globals.Config.PaymentType = PocketButler.Utils.DeliveryType.DeliveryTypeTableService;
						await Navigation.PushAsync(new ServiceTablePage(PageShowingEvent));
					}
				}
				else if (Globals.Config.CurrentVenue.settings.is_pickup_enabled.Equals("1"))
				{
					Globals.Config.PaymentType = PocketButler.Utils.DeliveryType.DeliveryTypePickup;
					await Navigation.PushAsync(new ServiceTablePage(PageShowingEvent));
				}
				else
				{
					Globals.Config.PaymentType = PocketButler.Utils.DeliveryType.DeliveryTypeTableService;
					await Navigation.PushAsync(new ServiceTablePage(PageShowingEvent));
				}
			};

			AbsoluteLayout.SetLayoutFlags(MainLayout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(MainLayout, new Rectangle(0, 0.7, 1, 0.9));
			UILayout.Children.Add(MainLayout);

			Content = UILayout;
		}

		private List<ExpandableTableListItem> GetListPaymentItems()
		{
			List<ExpandableTableListItem> listItems = new List<ExpandableTableListItem> ();
			List<PaymentTableItem> paymentList = App._DbManager.GetPaymentItems (Globals.Config.RestaurantId);

			while (paymentList.Count > 0) {
				int curTypeCnt = 0;

				String curMenuItemId = paymentList [0].MenuId;
				String menuName = paymentList[0].MenuName;
				String menuPrice = paymentList [0].ItemPrice;

				double dItemPrice = 0.0;
				double.TryParse (menuPrice, out dItemPrice);

				List<PaymentTableItem> menuItems = new List<PaymentTableItem> ();

				double dTotalPrice = 0.0;

				// Remove same items from the list
				for (int i = paymentList.Count - 1; i >= 0; i--) {
					if (paymentList [i].MenuId == curMenuItemId) {

						double.TryParse (paymentList [i].ItemPrice, out dItemPrice);

						dTotalPrice += dItemPrice;
						/*
						List<PaymentExtraItem> extraItems = App._DbManager.GetPaymentExtraItems (paymentList [i].ID);
						foreach (PaymentExtraItem extraItem in extraItems) {
							double dExtra = 0.0;
							double.TryParse (extraItem.Price, out dExtra);
							dTotalPrice += dExtra;
						}*/

						menuItems.Add (paymentList [i]);
						paymentList.RemoveAt (i);
						curTypeCnt++;
					}
				}

				if (curTypeCnt > 0) {
					ExpandableTableListItem item = new ExpandableTableListItem {
						Count = curTypeCnt,
						ItemName = menuName,
						IsExpanded = false,
						Items = menuItems,
						Price = dTotalPrice.ToString("0.00"),
						MenuId = curMenuItemId,
					};

					listItems.Add (item);
				}
			}

			return listItems;
		}
		#endregion

		#region EVENTS
		void MenuButton_Clicked()
		{
			if (MasterPage != null)
				MasterPage.IsPresented = true;
		}

		async void PaymentItem_Delete_Clicked(String restaurant_id, String item_id, int uid)
		{
			var result = await DisplayAlert("Warning", "Do you really want to delete this item?", "Yes", "No");
			if (result == true) {
				App._DbManager.RemovePaymentItem (uid);
				Globals.Config.PaymentInfo.Price = App._DbManager.GetPaymentTotalPrice (Globals.Config.RestaurantId).ToString("0.00");
				List<ExpandableTableListItem> dataList = GetListPaymentItems();
				if (dataList == null || dataList.Count == 0) {
					if (BackAppearingEvent != null)
						BackAppearingEvent.Invoke ();
					await Navigation.PopAsync ();
				}
				else
					PaymentListView.ListData = dataList;
				PaymentListView.ListData = dataList;
			}
		}

		async void PaymentItem_Edit_Item_Clicked(String restaurant_id, String item_id, int uid)
		{
			PaymentTableItem item = App._DbManager.GetPaymentItemWithId(uid);
			List<PaymentExtraItem> extras = App._DbManager.GetPaymentExtraItems (uid);
			List<PaymentTypeItem> types = App._DbManager.GetPaymentTypeItems (uid);
			if (item != null) {
				IsCustomizeRaised = true;
				var customPage = new VenueCustomizePage(item_id, "0", item.Customization, uid, true, extras, types, PageShowingEvent);
				await Navigation.PushAsync (customPage);
			}
		}
		#endregion
    }
}
