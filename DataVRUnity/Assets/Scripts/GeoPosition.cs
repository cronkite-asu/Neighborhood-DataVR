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

		//TODO: Move the configurations to a different script file and make this file clean
		public static MarkerObject selectedObject;



		//Read data file from android, ios, mac different ways


		private List<List<string>> readFileNonMobile()
		{
			string filename = "Nonprofits- Data Viz - Sheet1 copy.csv";
			string filePath = getFileName (filename);
			string readContents;
			using (StreamReader streamReader = new StreamReader(filePath, System.Text.Encoding.UTF8))
			{
				readContents = streamReader.ReadToEnd();
			}
			char[] splitChars = { '\n' };
			string[] array = readContents.Split (splitChars);

			List<List<string>> csvList = new List<List<string>> ();

			foreach (string sline in array) {
				List<string> colList = CsvReader.csvColumnSplitProcessor (sline);
				csvList.Add (colList);
			}			

			//Debug.Log ("DATAVR: File contents :\n" + result);
			return csvList;
			//return readContents;
		}

		private List<List<string>> readFile ()
		{
			//String file = "Assets/Data/Nonprofits- Data Viz - Sheet1 copy.csv";
			//String androidFilePath = Application.streamingAssetsPath + file;
			string filename = "Nonprofits- Data Viz - Sheet1 copy.csv";

			string androidFilePath = getFileName (filename);
			//string androidFilePath = System.IO.Path.Combine (Application.streamingAssetsPath, filename);

			Debug.Log (androidFilePath);
			WWW reader = new WWW (androidFilePath);
			while (!reader.isDone) {
			}

			Debug.Log ("DATAVR: file read ..\n");

			string result = System.Text.Encoding.UTF8.GetString (reader.bytes);
		
			char[] splitChars = { '\n' };
			string[] array = result.Split (splitChars);

			List<List<string>> csvList = new List<List<string>> ();

			foreach (string sline in array) {
				List<string> colList = CsvReader.csvColumnSplitProcessor (sline);
				csvList.Add (colList);
			}			

			Debug.Log ("DATAVR: File contents :\n" + result);
			return csvList;
		}


		private List<List<string>> getData()
		{
			switch (Application.platform) {
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return readFileNonMobile ();
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				return readFile ();
			}
			return null;
		}


		// Use this for initialization
		void Start ()
		{
			Debug.Log ("DATAVR: Start called...");
			name = "datavr project";
			statDefaultMarker = defaultMarker;
			try {
				List<List<string>> csvList = getData();
				populateMarkerList (csvList);

				//TODO: Figure out why on init is not called sometimes...
				Map.OnInitialized += () => {
					fetchGeoLocation (markerList);
				};
				if (markerTagging.Count == 0) {
					foreach (TypeMapping markerTag in markerTags) {
						markerTagging.Add (markerTag.name, markerTag.prefab);
					}
				}
			} catch (Exception ex) {
				Debug.Log ("DATAVR: DATAVREXCEPTION"+ ex.ToString ());
				Debug.LogException (ex, this);
			}
		}


		void OnLevelWasLoaded (int level)
		{
			Debug.Log ("On Level was loaded ...");
			// Plot the markers when the scene is loaded again.
			plotAllMarkers ();
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
			Debug.Log ("Geoposition - Perform action()");
			counter += 1;
			if (counter > 8) {
				if (canRun ()) {
					fetchHeightofAllMarkerLocations ();
				} else {
					//Plot the markers
					//plotAllMarkers ();
				}
			} 
			if (counter == 9) {
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

		private void plotAllMarkers ()
		{
			foreach (MarkerObject marker in markerList) {
				plotMarker (marker);
			}
		}

		private string getFileName(string fileName)
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
			markerList = new List<MarkerObject> ();

			foreach (List<string> colList in csvList) {
				string address = colList [2];
				string type = colList [1];
				string title = colList [0];

				//If type is not mentioned get the default type
				if (type == null || type.Length < 2) {
					type = DEFAULT_MARKER;
				}
				if (address != null && address.Length > 2) {

					MarkerObject marker = new MarkerObject (address, address, type);
					markerList.Add (marker);
				}
			}
		}


		void readDataFile ()
		{
			Debug.Log ("Reading data from file...");

			string filename = "Nonprofits- Data Viz - Sheet1 copy.csv";
			string androidFilePath = System.IO.Path.Combine (Application.streamingAssetsPath, filename);

			GeoCoding coder = new GeoCoding (Google_Maps_API_KEY);
			markerList = new List<MarkerObject> ();

			using (CsvReader reader = new CsvReader (androidFilePath)) {
				foreach (string[] values in reader.RowEnumerator) {
					//Debug.Log(string.Format( "Row {0} has {1} values.", reader.RowIndex, values.Length ));
					//foreach (string val in  values)
					{
						//Debug.Log ("val \t:" + val);
						//Address read and plot
						string address = values [2];
						string type = values [1];
						string title = values [0];

						//If type is not mentioned get the default type
						if (type == null || type.Length < 2) {
							type = DEFAULT_MARKER;
						}
						if (address != null && address.Length > 2) {
						
							MarkerObject marker = new MarkerObject (address, address, type);
							markerList.Add (marker);
						}
					}
				}
			}
		}

		//Method which fetches populates the lat long fields of the list of marker objects
		void fetchGeoLocation (List<MarkerObject> markerList)
		{
			GeoCoding coder = new GeoCoding (Google_Maps_API_KEY);
			Debug.Log ("No of marker in list while fetchint geo locaiton = " + (markerList.Count));
			foreach (MarkerObject marker in  markerList) {

				Location location = coder.GetGeoLocationFromAddress (marker.address);

				if (location != null) {
					marker.latitide = location.lat;
					marker.longitude = location.lng;

					Vector3 unityWorldCoordinate = fetchUnityWorldCoordinatesFromLatLong (marker.latitide, marker.longitude);

					//Assign the x nd z values to the marker object so that marker object can be later placed in the unity world
					marker.x = unityWorldCoordinate.x;
					marker.z = unityWorldCoordinate.z;
					//Later we fetch the y coordinate when the colliders are added and the map is completely initialized.
				}
			}
		}


		void fetchHeightofAllMarkerLocations ()
		{
			Debug.Log ("No of markers in the fetchHeightCall = " + markerList.Count); 
			foreach (MarkerObject marker in markerList) {
		
				double height = getHeightAtTile (new Vector3 ((float)marker.x, 0f, (float)marker.z));
				marker.y = (float)height;
				if (height > maxBuildingHeight) {
					maxBuildingHeight = height;
				}
			}
			Debug.Log ("Maximum height so far  = " + maxBuildingHeight);
		}

		private Vector3 fetchUnityWorldCoordinatesFromLatLong (double latitide, double longitude)
		{
			var llpos = new Vector2d (latitide, longitude);
			Vector3 pos = Conversions.GeoToWorldPosition (llpos, Map.CenterMercator, Map.WorldRelativeScale).ToVector3xz ();
			return pos;
		}



		void plotMarker (MarkerObject marker)
		{
			GameObject gameObject = statDefaultMarker;

			if (markerTagging.ContainsKey (marker.type)) {
				//Debug.Log ("Marker tag contains the key = " + marker.type);
				gameObject = markerTagging [marker.type];
			}

			Vector3 gameObjectPosition = new Vector3 ((float)marker.x, (float)maxBuildingHeight + 1, (float)marker.z);


			/*var gg = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gg.name = marker.address;
			gg.transform.position = gameObjectPosition;
		*/

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
			//Shoot ray from 50 points above the ground level
			position.y = 50;

			double height = 0;

			Vector3 targetLocation;

			if (Physics.Raycast (position, Vector3.down, out hit, distance)) {
				targetLocation = hit.point;
				//Debug.DrawLine (position, hit.point, Color.red, 100);
				Debug.Log ("Ray cast hit position " + hit.point);
				height = hit.point.y;
			}

			return height;
		}
	}
		
}
