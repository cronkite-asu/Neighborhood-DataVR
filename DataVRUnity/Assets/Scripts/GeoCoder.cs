using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace edu.asu.cronkite.datavr
{
    using UnityEngine;
    using System.Net;
    using System.IO;
    using System.Text;
	using System.Text.RegularExpressions;
    using System;

    public class GeoCoder : MonoBehaviour
	{

		protected string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
		public string API_KEY;
		private Regex latLongRegex;
	
		public GeoCoder (String apiKey)
		{
			this.API_KEY = apiKey;
			latLongRegex = new Regex(@"/^[+-]?\d+(\.\d+)?,[+-]?\d+(\.\d+)?$/");
		}

		// Make call to the google location api and get the lat long associated with the address.
		// Location object contains the latitude and longitude information of the palce.
		public Location GetGeoLocationFromAddress (string address)
		{
			Location loc = null;
			String url = baseUrl +"?key="+API_KEY+"&address=" + address;
			if(latLongRegex.Match(address).Success){
				Debug.Log("A latlong value is present in data file instead of the address!");
				string[] split_array = address.Split(',');
				loc.setLatitude(Convert.ToDouble(split_array[0]));
				loc.setLongitude(Convert.ToDouble(split_array[1]));
				return loc;
			} else{
				Debug.Log("Response URL:"+url);
				string jsonResponse = GET (url);

				RootObject result = JsonUtility.FromJson<RootObject> (jsonResponse);
				Debug.Log("Result Count:"+result.results.Count);
				if (result.results.Count > 0)
					loc = result.results [0].geometry.location;
				return loc;
			}
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
				WebResponse errorResponse = ex.Response;
				throw;
			}
		}

		// Method for validating the certificate of the web request.
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
