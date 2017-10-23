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

	// Use this for initialization
	void Start () {
		
        Debug.Log("Start called...");

        //TODO: Figure out why on init is not called...
        Map.OnInitialized += () =>
        {
        Debug.Log("Map initialized called...");
            foreach (var item in Coordinates)
            {
                var latLonSplit = item.Split(',');
                var llpos = new Vector2d(double.Parse(latLonSplit[0]), double.Parse(latLonSplit[1]));
               // Vector3 pos = Conversions.GeoToWorldPosition(llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz();
				Vector3 pos = Conversions.GeoToWorldPosition(llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz();
				Debug.Log(pos);

				var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
               gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.y);

			}
        };
	}
	
}
