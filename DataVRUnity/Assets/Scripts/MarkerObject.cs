namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Marker object.
	/// 	Class used for holding the data related to the makers displayed in the UI 
	/// 	Includes 1)Unity location 2)Real world location 3)Details of the position 
	/// 	If modifying the asset to represent different application - this class needs to be modified to hold relevant information
	/// </summary>
	[System.Serializable]
	public class MarkerObject
	{
		// Unity world coordinates
		public float x;
		public float y;
		public float z;

		// Type assiciated with each marker object - type defines the different game objects that could be rendered
		public string type;

		// Real world geo coordinates
		public double latitide;
		public double longitude;

		// Additional information - depending on the application specific
		public string address;
		public string other;
		public string title;
		public string telephone;
		public string url;

		public MarkerObject (string address, string title, string type, string telephone, string url)
		{
			this.address = address;
			this.title = title;
			this.type = type;
			this.telephone = telephone;
			this.url = url;
		}

		// Modify this method to change the content to be displayed on the marker inforamtion poped up on clicking
		public string getContentDisplayText()
		{
			return type+"\n"+ address +"\n" + telephone + "\n"+ url;
		}
	}
}