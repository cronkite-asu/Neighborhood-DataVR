namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Utilities;
	using Mapbox.Unity.Map;
	using Mapbox.Utils;
	using Mapbox.Unity.MeshGeneration.Modifiers;
	using UnityEngine.SceneManagement;
	using System.IO;
	using System.Collections.Generic;
	using System;


	[System.Serializable]
	public class GeoPosition : MonoBehaviour
	{
		[System.Serializable]
		public struct TypeMapping
		{
			public string name;
			public GameObject prefab;
		}

		private static GeoPosition instance;

		private GeoPosition ()
		{
		}

		public static GeoPosition Instance {
			get {
				if (instance == null) {
					instance = new GeoPosition ();
		
				}
				return instance;
			}
		}

		public AbstractMap Map;
		private static Dictionary<string, GameObject> markerTagging = new Dictionary<string, GameObject> ();
		public TypeMapping[] markerTags;
		public GameObject defaultMarker;
		private static int counter = 1;
		private static GameObject statDefaultMarker;
	

		private static double maxBuildingHeight = 0;
		private static string DEFAULT_MARKER = "default";
		private static List<MarkerObject> markerList;
		public string name;
		public string Google_Maps_API_KEY;
		public string FileName;
		public TextAsset textFile;
		public float GazeTime;
		private static float GazeTimeLimit;

		//TODO: Move the configurations to a different script file and make this file clean
		public static MarkerObject selectedObject;


		public float getGazeTimeLimit()
		{
			return GazeTimeLimit;
		}

		//Read data file from android, ios, mac different ways
		private List<List<string>> readFileNonMobile ()
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

		private List<List<string>> readTextAsset()
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


		// Use this for initialization
		void Start ()
		{
			Debug.Log ("DATAVR App: Start called...");
		
			statDefaultMarker = defaultMarker;
			GazeTimeLimit = GazeTime;
			try {
				List<List<string>> csvList = getData ();
				populateMarkerList (csvList);

				Map.OnInitialized += () => {
					fetchGeoLocation (markerList);
				};
				if (markerTagging.Count == 0) {
					foreach (TypeMapping markerTag in markerTags) {
						markerTagging.Add (markerTag.name, markerTag.prefab);
					}
				}
			} catch (Exception ex) {
				Debug.LogException (ex, this);
			}
		}

		public void performOnce ()
		{
			if (counter == 0) {
				counter = counter + 1;

				foreach (TypeMapping markerTag in markerTags) {
					markerTagging.Add (markerTag.name, markerTag.prefab);
				}
					
				readDataFile ();
				fetchGeoLocation (markerList);
			}
		}

		public void performAction ()
		{
			counter += 1;
			Debug.Log ("DATAVR :Perform action() - Count : " + counter);
			if (counter > 8) {
				if (canRun ()) {
					fetchHeightofAllMarkerLocations ();
				} else {
					//Plot the markers
					//TODO:Test the behaviour uncommenting or removing the next line for slippery map
					//plotAllMarkers ();
				}
			} 
			if (counter == 9) {
				
				fetchHeightofAllMarkerLocations ();
				//Plot the markers
				plotAllMarkers ();
			} 	
		}

			
		//Return whether the run method can be called or not
		public bool canRun ()
		{
			//Run untill all the building layers are called.
			return !(maxBuildingHeight > 0);
		}

		// Plot all the marker objects
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

		void populateMarkerList (List<List<string>> csvList)
		{
			Debug.Log ("DATAVR : populate Marker List called...");
			markerList = new List<MarkerObject> ();

			foreach (List<string> colList in csvList) {
				string address = colList [2];
				string type = colList [1];
				string title = colList [0];
				string telephone = colList [3];
				string url = colList [4];


				//If type is not mentioned get the default type
				if (type == null || type.Length < 2) {
					type = DEFAULT_MARKER;
				}
				if (address != null && address.Length > 2) {

					MarkerObject marker = new MarkerObject (address, title, type, telephone, url);
					markerList.Add (marker);
				}
			}
			Debug.Log ("DATAVR: Populate marker list completed...");
		}


		void readDataFile ()
		{
			Debug.Log ("Reading data from file...");

			string filename = "Nonprofits- Data Viz - Sheet1 copy.csv";
			string androidFilePath = System.IO.Path.Combine (Application.streamingAssetsPath, filename);

			GeoCoder coder = new GeoCoder (Google_Maps_API_KEY);
			markerList = new List<MarkerObject> ();

			using (CsvReader reader = new CsvReader (androidFilePath)) {
				foreach (string[] values in reader.RowEnumerator) {
					{
						//Address read and plot
						string address = values [2];
						string type = values [1];
						string title = values [0];
						string telephone = values [3];
						string url = values [4];

						//If type is not mentioned get the default type
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

		//Method which fetches populates the lat long fields of the list of marker objects
		void fetchGeoLocation (List<MarkerObject> markerList)
		{
			GeoCoder coder = new GeoCoder (Google_Maps_API_KEY);
			Debug.Log ("No of marker in list while fetchint geo locaiton = " + (markerList.Count));
			foreach (MarkerObject marker in  markerList) {

				Location location = coder.GetGeoLocationFromAddress (marker.address);

				if (location != null) {
					marker.latitide = location.lat;
					marker.longitude = location.lng;

					Vector3 unityWorldCoordinate = getUnityWorldCoordinatesFromLatLong (marker.latitide, marker.longitude);

					//Assign the x nd z values to the marker object so that marker object can be later placed in the unity world
					marker.x = unityWorldCoordinate.x;
					marker.z = unityWorldCoordinate.z;
					//Later we fetch the y coordinate when the colliders are added and the map is completely initialized.
				}
			}
		}


		/// <summary>
		/// Fetchs the maximum height of buildings at marker locations for all marker locations
		///  For each marker object location, raycast downwards to find the height of the building at that point
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
			Debug.Log ("Maximum height so far  = " + maxBuildingHeight);
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

			// Get the custom gameobject to be instantiated for marker based on the type if it exists
			if (markerTagging.ContainsKey (marker.type)) {
				gameObject = markerTagging [marker.type];
			}

			// Instantiate the game object at the specified unity location and add the trigger scripts for gaze and click actions
			Vector3 gameObjectPosition = new Vector3 ((float)marker.x, (float)maxBuildingHeight + 1, (float)marker.z);
			GameObject placedMarker = Instantiate (gameObject, gameObjectPosition, Quaternion.identity);
			placedMarker.name = marker.title;
			MarkerEventTrigger trigger = placedMarker.AddComponent<MarkerEventTrigger> ();
			trigger.setMarkerObj (marker);
		}

		//Get the height of the given point by casting ray downwards
		double getHeightAtTile (Vector3 position)
		{
			RaycastHit hit;
			float distance = 100f;
			//Shoot ray from 50 points above the ground level - assumption is that buildings will not be more that 50 units
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
