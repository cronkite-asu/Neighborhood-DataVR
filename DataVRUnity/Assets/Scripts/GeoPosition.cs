using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Map;
    using Mapbox.Utils;

public class GeoPosition : MonoBehaviour {

	
    public AbstractMap Map;
    public List<string> Coordinates;

	private static string DEFAULT_MARKER = "default";

	// Use this for initialization
	void Start () {
	
		//TODO: Figure out why on init is not called sometimes...
		Map.OnInitialized += () => {
			readCSVAndPlotMarkers();
		};

        Debug.Log("Start called...");

       
	}

	void readCSVAndPlotMarkers()
	{
		GeoCoding coder = new GeoCoding ();

		using ( CsvReader reader = new CsvReader( "Assets/Data/Nonprofits- Data Viz - Sheet1.csv" ) )
		{
			foreach( string[] values in reader.RowEnumerator )
			{
				//Debug.Log(string.Format( "Row {0} has {1} values.", reader.RowIndex, values.Length ));
				//foreach (string val in  values)
				{
					//Debug.Log ("val \t:" + val);
					//Address read and plot
					string address =  values[2];
					string type = values [1];
					//If type is not mentioned get the default type

					if (type == null || type.Length < 2) {
						type = DEFAULT_MARKER;
					}
					if (address != null && address.Length > 2) {
						//Plot the markers
						//Debug.Log ("address - " + address);
						Location location = coder.GetGeoLocationFromAddress (address);
						if (location != null) {
							plotMarkers (location.lat, location.lng, address);

						}
					}
				}
			}
		}
	}

	void plotMarkers(double latitide, double longitude, string address)
	{
		Debug.Log("Plotting markers in the position " + latitide+ ", "+ longitude);

		var llpos = new Vector2d(latitide, longitude);
		// Vector3 pos = Conversions.GeoToWorldPosition(llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz();
		Vector3 pos = Conversions.GeoToWorldPosition(llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz();
		Debug.Log("unity position - "+ pos);

		var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		gg.name = address;
		gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.z);
			
	}
}
