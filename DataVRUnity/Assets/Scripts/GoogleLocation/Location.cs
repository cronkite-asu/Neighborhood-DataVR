using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location {
	public double lat;
	public double lng;

	public void setLatitude(double latitude){
		lat = latitude;
	}

	public double getLatitude(){
		return lat;
	}

	public void setLongitude(double longitude){
		lng = longitude;
	}

	public double getLongitude(){
		return lng;
	}
}


