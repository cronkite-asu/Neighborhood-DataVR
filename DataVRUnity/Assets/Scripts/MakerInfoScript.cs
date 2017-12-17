namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class MakerInfoScript : EventTrigger
	{

		// Use this for initialization
		void Start ()
		{
			//Set the text with proper information
			MarkerObject marker = GeoPosition.selectedObject;
			GameObject markerText = GameObject.Find ("MarkerInfoText");
			Text text = markerText.GetComponent<Text> ();
			//text.text = text.text + marker.address;	
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void backButtonClicked ()
		{
			Debug.Log ("Back button clicked...");
			SceneManager.LoadScene ("MapScene");
		}
	}
}
