namespace edu.asu.cronkite.datavr
{
    using UnityEngine;
    using System.Collections.Generic;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using System;


    [System.Serializable]
	public class GeoPosition : MonoBehaviour
	{
		private static GeoPosition instance = null;

		public static GeoPosition Instance {
			get {
				if (instance == null) {
					instance = new GeoPosition ();
				}
				return instance;
			}
		}

		public AbstractMap Map;
		private static Dictionary<string, GameObject> markerTagging;
		private static int counter = 1;
		private GameObject statDefaultMarker;
		private double maxBuildingHeight = 0;
		private string DEFAULT_MARKER = "default";
		private List<MarkerObject> markerList;
		public static MarkerObject selectedObject;


		public float getGazeTimeLimit ()
		{
			return Configuration.gazeTimeLimit;
		}

		//Read data file from android, ios, mac different ways
		/*private List<List<string>> readFileNonMobile ()
		{
			string filePath = getFileName (FileName);
			string readContents;
			using (StreamReader streamReader = new StreamReader (filePath, System.Text.Encoding.UTF8)) {
				readContents = streamReader.ReadToEnd ();
			}
			char[] splitChars = { '\n' };
			string[] array = readContents.Split (splitChars);

			List<List<string>> csvList = new List<List<string>> ();

			foreach (string sline in array) {
				List<string> colList = CsvReader.csvColumnSplitProcessor (sline);
				csvList.Add (colList);
			}

			return csvList;
		}

		/*private List<List<string>> readTextAsset()
		{
			String readContents = textFile.text;
			char[] splitChars = { '\n' };
			string[] array = readContents.Split (splitChars);

			List<List<string>> csvList = new List<List<string>> ();

			foreach (string sline in array) {
				List<string> colList = CsvReader.csvColumnSplitProcessor (sline);
				csvList.Add (colList);
			}

			return csvList;
		}

*/

		/*
		private List<List<string>> readFileMobile ()
		{
			string deviceFilePath = getFileName (FileName);

			Debug.Log (deviceFilePath);
			WWW reader = new WWW (deviceFilePath);
			while (!reader.isDone) {
			}

			string result = System.Text.Encoding.UTF8.GetString (reader.bytes);

			char[] splitChars = { '\n' };
			string[] array = result.Split (splitChars);

			List<List<string>> csvList = new List<List<string>> ();

			foreach (string sline in array) {
				List<string> colList = CsvReader.csvColumnSplitProcessor (sline);
				csvList.Add (colList);
			}

			return csvList;
		}


		private List<List<string>> getData ()
		{
			switch (Application.platform) {
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return readTextAsset ();
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				return readTextAsset ();
			}
			return null;
		}

		*/

		void Start ()
		{
			// Debug.Log ("DATAVR App: Start called...");
			statDefaultMarker = Configuration.statDefaultMarker;
			maxBuildingHeight = Configuration.sMarkerHeight;
			markerTagging = Configuration.markerTagging;

			try {
				markerList = Configuration.getFilteredList ();

				Map.OnInitialized += () => {
					fetchGeoLocation (markerList);
					plotAllMarkers ();
				};

			} catch (Exception ex) {
				Debug.LogException (ex, this);
			}
		}


		public void performAction ()
		{
			// Method to perform some actions when the tile is loaded completely.
			// When fixed number of tiles are rendered we can perform some operations is finding
			// height of the building at the generated tile and the marker height can be adjusted
			// depending on the height of all the tile dynamically.
		}

		public static void updateCounter (int count)
		{
			counter = count;
		}

		// Return whether the run method can be called or not.
		public bool canRun ()
		{
			// Run untill all the building layers are called.
			return !(maxBuildingHeight > 0);
		}

		// Plot all the marker objects.
		private void plotAllMarkers ()
		{

			foreach (MarkerObject marker in markerList) {
				plotMarker (marker);
			}
		}

		/// <summary>
		/// Returns the file name depending on the platform on which the app is running
		/// </summary>
		/// <returns>The file name.</returns>
		/// <param name="fileName">File name.</param>
		private string getFileName (string fileName)
		{
			string filename = fileName;
			switch (Application.platform) {

			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:

				filename = System.IO.Path.Combine (Application.streamingAssetsPath, fileName);
				break;
			default:
				filename = "Assets/StreamingAssets/" + fileName;
				break;
			}
			return filename;
		}

		// Method for reading data from the file when raw file is imported instead of TestAsset
		private void readDataFile ()
		{
			// Debug.Log ("Reading data from file...");

			string filename = ""; // Provide the file name here to read raw file
			string androidFilePath = System.IO.Path.Combine (Application.streamingAssetsPath, filename);

			markerList = new List<MarkerObject> ();

			using (CsvReader reader = new CsvReader (androidFilePath)) {
				foreach (string[] values in reader.RowEnumerator) {
					{
						// Address read and plot.
						string address = values [2];
						string type = values [1];
						string title = values [0];
						string telephone = values [3];
						string url = values [4];

						// If type is not mentioned get the default type.
						if (type == null || type.Length < 2) {
							type = DEFAULT_MARKER;
						}
						if (address != null && address.Length > 2) {

							MarkerObject marker = new MarkerObject (address, title, type, telephone, url);
							markerList.Add (marker);
						}
					}
				}
			}
		}


		// Method which fetches populates the lat long fields of the list of marker objects.
		void fetchGeoLocation (List<MarkerObject> markerList)
		{
			GeoCoder coder = new GeoCoder (Configuration.getGoogleMapsApiKey ());
			foreach (MarkerObject marker in  markerList) {

				Location location = coder.GetGeoLocationFromAddress (marker.address);

				if (location != null) {
					marker.latitide = location.lat;
					marker.longitude = location.lng;

					Vector3 unityWorldCoordinate = getUnityWorldCoordinatesFromLatLong (marker.latitide, marker.longitude);

					// Assign the x nd z values to the marker object so that marker object can be later placed in the unity world.
					marker.x = unityWorldCoordinate.x;
					marker.z = unityWorldCoordinate.z;
					// Later we fetch the y coordinate when the colliders are added and the map is completely initialized.
				}
			}
		}

		/// <summary>
		/// Fetches the maximum height of buildings at marker locations for all marker locations.
		///  For each marker object location, raycast downwards to find the height of the building at that point.
		/// </summary>
		void fetchHeightofAllMarkerLocations ()
		{
			foreach (MarkerObject marker in markerList) {
				double height = getHeightAtTile (new Vector3 ((float)marker.x, 0f, (float)marker.z));
				marker.y = (float)height;

				if (height > maxBuildingHeight) {
					maxBuildingHeight = height;
				}
			}
			// Debug.Log ("Maximum height so far  = " + maxBuildingHeight);
		}

		/// <summary>
		/// Returns the unity world coordinates for latitide and longitude.
		/// </summary>
		/// <returns>The unity world coordinates from lat long.</returns>
		/// <param name="latitide">Latitide.</param>
		/// <param name="longitude">Longitude.</param>
		private Vector3 getUnityWorldCoordinatesFromLatLong (double latitide, double longitude)
		{
			var llpos = new Vector2d (latitide, longitude);
			Vector3 pos = Conversions.GeoToWorldPosition (llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz ();
			return pos;
		}

		/// <summary>
		/// Plots the marker.
		/// </summary>
		/// <param name="marker">Marker.</param>
		void plotMarker (MarkerObject marker)
		{
			GameObject gameObject = statDefaultMarker;

			// Get the custom gameobject to be instantiated for marker based on the type if it exists.
			if (markerTagging.ContainsKey (marker.type)) {
				gameObject = markerTagging [marker.type];
			}

			// Instantiate the game object at the specified unity location and add the trigger scripts for gaze and click actions.
			Vector3 gameObjectPosition = new Vector3 ((float)marker.x, (float)maxBuildingHeight + 1, (float)marker.z);
			GameObject placedMarker = Instantiate (gameObject, gameObjectPosition, Quaternion.identity);
			placedMarker.name = marker.title;
			MarkerEventTrigger trigger = placedMarker.AddComponent<MarkerEventTrigger> ();
			trigger.setMarkerObj (marker);
		}

		// Get the height of the given point by casting ray downwards.
		double getHeightAtTile (Vector3 position)
		{
			RaycastHit hit;
			float distance = 100f;
			// Shoot ray from 50 points above the ground level - assumption is that buildings will not be more that 50 units.
			position.y = 50;
			double height = 0;
			Vector3 targetLocation;

			if (Physics.Raycast (position, Vector3.down, out hit, distance)) {
				targetLocation = hit.point;
				height = hit.point.y;
			}
			return height;
		}
	}
}
