using System;
using Xamarin.Forms;
using PocketButler;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Maps;

[assembly: ExportRenderer (typeof(CustomMapView), typeof(PocketButler.Droid.Renderer.MapViewRenderer))]
namespace PocketButler.Droid.Renderer
{
	public class MapViewRenderer : MapRenderer
	{
		public MapViewRenderer ()
		{
		}

		bool _isDrawnDone;

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			var androidMapView = (MapView)Control;
			var formsMap = (CustomMapView)sender;

			if ((e.PropertyName.Equals ("VisibleRegion") && !_isDrawnDone) || formsMap.ForceRedraw) {
				formsMap.ForceRedraw = false;
				androidMapView.Map.Clear ();

				//androidMapView.Map.MarkerClick += HandleMarkerClick;
				androidMapView.Map.InfoWindowClick += HandlerInfoWindowClick;
				androidMapView.Map.MyLocationEnabled = formsMap.IsShowingUser;

				var formsPins = formsMap.CustomPins;

				foreach (var formsPin in formsPins) {
					var markerWithIcon = new MarkerOptions ();

					markerWithIcon.SetPosition (new LatLng (formsPin.Position.Latitude, formsPin.Position.Longitude));
					markerWithIcon.SetTitle (formsPin.Label);
					markerWithIcon.SetSnippet (formsPin.Address);

					if (!string.IsNullOrEmpty (formsPin.PinIcon))
						markerWithIcon.InvokeIcon (BitmapDescriptorFactory.FromAsset (String.Format ("{0}.png", formsPin.PinIcon)));
					else
						markerWithIcon.InvokeIcon (BitmapDescriptorFactory.DefaultMarker ());

					Marker marker = androidMapView.Map.AddMarker (markerWithIcon);
					marker.ShowInfoWindow ();
				}

				_isDrawnDone = true;

			}
		}

		void HandleMarkerClick (object sender, Android.Gms.Maps.GoogleMap.InfoWindowClickEventArgs e)
		{
			var marker = e.P0;
			marker.ShowInfoWindow ();

			var myMap = this.Element as CustomMapView;

			var formsPin = new CustomPin {
				Label = marker.Title,
				Address = marker.Snippet,
				Position = new Position (marker.Position.Latitude, marker.Position.Longitude),
			};
		}

		void HandlerInfoWindowClick(object sender, Android.Gms.Maps.GoogleMap.InfoWindowClickEventArgs e){
			var marker = e.P0;

			var myMap = this.Element as CustomMapView;

			var formsPin = new CustomPin {
				Label = marker.Title,
				Address = marker.Snippet,
				Position = new Position (marker.Position.Latitude, marker.Position.Longitude),
			};

			myMap.SelectedPin = formsPin;
			if (myMap.SelectedPinChanged != null)
				myMap.SelectedPinChanged.Invoke ();
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);

			//NOTIFY CHANGE

			if (changed) {
				_isDrawnDone = false;
			}
		}
	}
}