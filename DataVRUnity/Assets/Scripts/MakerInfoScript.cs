namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Script for showing the details canvas when the click event happens.
	/// </summary>
	public class MakerInfoScript : EventTrigger
	{
		/// <summary>
		/// Gets the vector from end.
		/// Finds the distance of the point at specified units from the end vector in the direction of the line
		/// </summary>
		/// <returns>The vector from end.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="distFromEnd">Dist from end.</param>
		private static Vector2 getVectorFromEnd(Vector2 start, Vector2 end, int distFromEnd)
		{
			Vector2 result;
			float vecDist = Vector2.Distance (start, end);
			float ratio = (vecDist + distFromEnd) / vecDist;
			float x = ((1 - ratio) * start.x) + (ratio * end.x);
			float y = ((1 - ratio) * start.y) + (ratio * end.y);
			result = new Vector2 (x, y);
			return result;
		}

		/// <summary>
		/// Gets the vector from start.
		/// Finds the point at a specified distance from the start point along the line.
		/// </summary>
		/// <returns>The vector from start.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="distFromStart">Dist from start.</param>
		private static Vector2 getVectorFromStart(Vector2 start, Vector2 end, int distFromStart)
		{
			float vecDist = Vector2.Distance (start, end);
			float ratio = distFromStart / vecDist;
			float x = ((1 - ratio) * start.x) + (ratio * end.x);
			float y = ((1 - ratio) * start.y) + (ratio * end.y);
			return new Vector2 (x, y);
		}

		/// <summary>
		/// Shows the marker details.
		/// </summary>
		/// <param name="selectedMarker">Selected marker.</param>
		public static void showMarkerDetails(MarkerObject selectedMarker)
		{
 			GameObject obj = GameObject.Find("MarkerDetails");

			GameObject timerCanvas = obj.transform.Find("InfoCanvas").gameObject;
			//Show the hidden canvas information
			//Draw the line and position the canvas at the right position and rotate the canvas to show billboard effect
			timerCanvas.SetActive(true);


			GameObject infoTitle = GameObject.Find ("MarkerInfoTitle");
			GameObject infoContents = GameObject.Find ("MarkerInfoDetails");
			Text titleText = infoTitle.GetComponent<Text> ();
			titleText.text = selectedMarker.title;
			Text detailText = infoContents.GetComponent<Text> ();
			detailText.text = selectedMarker.getContentDisplayText();



			GameObject mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
			Vector3 cameraPos = mainCamera.transform.position;

			Vector2 camVec = new Vector2 (cameraPos.x, cameraPos.z);
			Vector2 markerVec = new Vector2 (selectedMarker.x, selectedMarker.z);

			Vector2 transPos = getVectorFromEnd (camVec, markerVec,300);
			obj.transform.position = new Vector3 (transPos.x, 200, transPos.y);

			GameObject playerGameObj = GameObject.Find ("Player");
			moveCamera (playerGameObj, camVec, markerVec);
		}


		private static void moveCamera(GameObject cameraObj, Vector2 camPos, Vector3 markerPos)
		{
			if (Vector2.Distance (camPos, markerPos) > 50) {
				Vector2 newCamPos = getVectorFromStart (camPos, markerPos, 50);
				cameraObj.transform.position = new Vector3 (newCamPos.x,30,newCamPos.y);
			}
		}

		// Use this for initialization
		void Start ()
		{
			//Set the text with proper information
			//MarkerObject marker = GeoPosition.selectedObject;
			//GameObject markerText = GameObject.Find ("MarkerInfoDetails");
			//Text text = markerText.GetComponent<Text> ();
			//text.text = text.text + marker.address;
		}

		//Method with action to perfom on closing the popup dialog
		//Hide the pop up dialog shown for the displaying marker information.
		public void closeButtonClicked ()
		{
			GameObject infoCanvas = GameObject.Find ("InfoCanvas");
			infoCanvas.SetActive (false);
		}
	}
}
