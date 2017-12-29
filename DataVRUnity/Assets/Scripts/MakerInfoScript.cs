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

		private static Vector2 getVectorFromStart(Vector2 start, Vector2 end, int distFromStart)
		{
			float vecDist = Vector2.Distance (start, end);
			float ratio = distFromStart / vecDist;
			float x = ((1 - ratio) * start.x) + (ratio * end.x);
			float y = ((1 - ratio) * start.y) + (ratio * end.y);
			return new Vector2 (x, y);	
		}


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
			MarkerObject marker = GeoPosition.selectedObject;
			GameObject markerText = GameObject.Find ("MarkerInfoText");
			Text text = markerText.GetComponent<Text> ();
			//text.text = text.text + marker.address;	
		}
			
		public void backButtonClicked ()
		{
			Debug.Log ("Back button clicked...");
			GameObject infoCanvas = GameObject.Find ("InfoCanvas"); 
			infoCanvas.SetActive (false);
		}
	}
}
