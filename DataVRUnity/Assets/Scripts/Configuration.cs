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


	public class Configuration : MonoBehaviour
	{

		[System.Serializable]
		public struct TypeMapping
		{
			public string name;
			public GameObject prefab;
		}

		public TextAsset textFile;
		private static List<MarkerObject> markerList;
		public TypeMapping[] markerTags;
		public GameObject defaultMarker;
		private static int counter = 1;
		public static GameObject statDefaultMarker;
		public static Dictionary<string, GameObject> markerTagging = new Dictionary<string, GameObject> ();
		private static string DEFAULT_MARKER = "default";
		private static List<MarkerObject> filteredList;
		private static List<String> choosenFilters;
		public Canvas filterCanvas;
		public string googleMapsApiKey;
		public float markerHeight;
		public static float sMarkerHeight;
		public float gazeTime;
		public static float gazeTimeLimit;
		private static string googleMapKey;

		private Configuration ()
		{
		}

		private static Configuration instance = null;

		public static Configuration Instance {
			get {
				if (instance == null) {
					instance = new Configuration ();
				}
				return instance;
			}
		}

		public static string getGoogleMapsApiKey ()
		{
			return googleMapKey;
		}

		private static SortedDictionary<string, bool> filterDict;

		public void filterValueChanged (string key, bool value)
		{
			filterDict [key] = value;
		}

		public static List<MarkerObject> getFilteredList ()
		{
			return filteredList;
		}

		public static void filterSelection ()
		{
			HashSet<string> selectedValues = new HashSet<string> ();
			foreach (KeyValuePair<string, bool> entry in filterDict) {
				// do something with entry.Value or entry.Key
				if (entry.Value)
					selectedValues.Add (entry.Key);
			}

			filteredList = new List<MarkerObject> ();
			foreach (MarkerObject marker in markerList) {
				if (selectedValues.Contains (marker.type))
					filteredList.Add (marker);
			}
		}

		// Use this for initialization
		void Start ()
		{
			// Read the files only when the scene is loaded for the first time
			if (filterDict == null) {
				statDefaultMarker = defaultMarker;
				sMarkerHeight = markerHeight;
				gazeTimeLimit = gazeTime;

				filterDict = new SortedDictionary<string, bool> ();
				List<List<string>> csvList = getData ();
				populateMarkerList (csvList);

				foreach (MarkerObject markerObj in markerList) {
					filterDict [markerObj.type] = true;
				}

				foreach (Configuration.TypeMapping markerTag in markerTags) {
					markerTagging.Add (markerTag.name, markerTag.prefab);
				}
			}



			//Once the filters are obtained. display them in the canvas as checkboxes
			FilterScript filterScript = filterCanvas.GetComponent<FilterScript> ();
			filterScript.addCheckBoxesForTypes (filterDict);
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


		private List<List<string>> readTextAsset ()
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
	}
}