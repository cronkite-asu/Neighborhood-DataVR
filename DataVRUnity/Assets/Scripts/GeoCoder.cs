using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Net;
	using System.IO;
	using System.Text;
	using System;
	using UnityEngine.Networking;

	public class GeoCoder : MonoBehaviour
	{

		protected string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=";
		protected string key = "&key=";

		public string API_KEY;

		public GeoCoder (String apiKey)
		{
			this.API_KEY = apiKey;
		}

		// Make call to the google location api and get the lat long associated with the address
		// Location object contains the latitude and longitude information of the palce
		public Location GetGeoLocationFromAddress (string address)
		{
			Location loc = null;
			string url = baseUrl + address + key + API_KEY;
			string jsonResponse = GET (url);

			RootObject result = JsonUtility.FromJson<RootObject> (jsonResponse);
			if (result.results.Count > 0)
				loc = result.results [0].geometry.location; 
			return loc;
		}



		string GET (string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
			try {
				WebResponse response = request.GetResponse ();
				using (Stream responseStream = response.GetResponseStream ()) {
					StreamReader reader = new StreamReader (responseStream, Encoding.UTF8);
					return reader.ReadToEnd ();
				}
			} catch (WebException ex) {
				Debug.Log ("Web exception" + ex.ToString ());
				WebResponse errorResponse = ex.Response;
				using (Stream responseStream = errorResponse.GetResponseStream ()) {
					StreamReader reader = new StreamReader (responseStream, Encoding.GetEncoding ("utf-8"));
					string errorText = reader.ReadToEnd ();
					// log errorText
					Debug.Log ("Exception in getting the geo coding for the url " + url);
				}
				throw;
			}
		}


		// Method for validating the certificate of the web request
		public bool MyRemoteCertificateValidationCallback (System.Object sender,
		                                                   X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool isOk = true;
			// If there are errors in the certificate chain,
			// look at each error to determine the cause.
			if (sslPolicyErrors != SslPolicyErrors.None) {
				for (int i = 0; i < chain.ChainStatus.Length; i++) {
					if (chain.ChainStatus [i].Status == X509ChainStatusFlags.RevocationStatusUnknown) {
						continue;
					}
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
						break;
					}
				}
			}
			return isOk;
		}
	}
}
