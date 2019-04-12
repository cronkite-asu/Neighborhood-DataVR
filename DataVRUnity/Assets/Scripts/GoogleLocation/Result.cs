using System.Collections;

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Result {
	public List<AddressComponent> address_components;
	public string formatted_address;
	public Geometry geometry;
	public string place_id;
	public List<string> types;
}
