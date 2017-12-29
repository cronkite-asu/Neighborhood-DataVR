namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[System.Serializable]
	public class MarkerObject
	{
		public float x;
		public float y;
		public float z;

		public string type;

		public double latitide;
		public double longitude;
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

		public string getContentDisplayText()
		{
			return type+"\n"+ address +"\n" + telephone + "\n"+ url;
		}
	}
}