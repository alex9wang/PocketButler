using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PocketButler
{
	public class MenuCategory
	{
		public string category_id { get; set; }
		public string category_name { get; set; }
		public string is_subcat_exists { get; set; }
		[DataMember(Name = "subcategories")]
		public List<SubCategory> subcategories { get; set; }
		public string is_menuitem_exist { get; set; }
		[DataMember(Name = "menuitems")]
		public List<MenuItem> menuitems { get; set; }
	}

	public class Settings
	{
		public object is_pickup_enabled { get; set; }
		public object is_tableservice_enabled { get; set; }
		public string is_tabservice_enabled { get; set; }
	}

	public class OpeningHours
	{
		public string day { get; set; }
		public string timing { get; set; }
		public string is_closed { get; set; }
	}

	public class RestaurantInfo
	{
		public string id { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public string description { get; set; }
		public string image { get; set; }
		public string image_large { get; set; }
		public string phone_no { get; set; }
		public string email { get; set; }
		public string website { get; set; }
		public string lat { get; set; }
		public string lng { get; set; }
		public string distance { get; set; }
		public string is_favourite { get; set; }
		public string is_offline { get; set; }
		public string liquor_license { get; set; }
		[DataMember(Name = "menu_categories")]
		public List<MenuCategory> menu_categories { get; set; }
		[DataMember(Name = "settings")]
		public Settings settings { get; set; }
		public string mapofvenue_image { get; set; }
		[DataMember(Name = "opening_hours")]
		public List<OpeningHours> opening_hours { get; set; }
		public string merchant_stripe_publishable_key { get; set; }
		public string pb_stripe_publishable_key { get; set; }
	}
}

