using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using System;

public class GeoCoding : MonoBehaviour {

	protected string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=";
	protected string key = "&key=";

	public string API_KEY;

	// Make call to the google location api and get the lat long associated with the address
	// Location object contains the latitude and longitude information of the palce
	public Location GetGeoLocationFromAddress(string address)
	{
		Location loc = null;
		string url = baseUrl + address +key+ API_KEY;
		string jsonResponse = GET (url);
		RootObject result  = JsonUtility.FromJson<RootObject> (jsonResponse);
		if(result.results.Count >0)
			loc =result.results[0].geometry.location; 
		return loc;
	}



	string GET(string url) 
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		try {
			WebResponse response = request.GetResponse();
			using (Stream responseStream = response.GetResponseStream()) {
				StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
				return reader.ReadToEnd();
			}
		}
		catch (WebException ex) {
			WebResponse errorResponse = ex.Response;
			using (Stream responseStream = errorResponse.GetResponseStream())
			{
				StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
				string errorText = reader.ReadToEnd();
				// log errorText
				Debug.Log("Exception in getting the GEO CODING for the url "+ url);
			}
			throw;
		}
	}
}
