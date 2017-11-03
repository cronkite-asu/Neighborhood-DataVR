namespace edu.asu.cronkite.datavr
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarkerObject {
	public double x;
	public double y;
	public double z;

	public string type;

	public double latitide;
	public double longitude;
	public string address;
	public string other;
	public string title;

	public MarkerObject(string address, string title, string type)
	{
		this.address = address;
		this.title = title;
		this.type = type;
	}
}
}