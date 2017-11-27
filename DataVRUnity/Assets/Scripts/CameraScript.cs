namespace  edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CameraScript : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
			Debug.Log ("CameraScript: Start() method called..");
			updatePositionCalledFromInfoScene ();
		}
	

		private void updatePositionCalledFromInfoScene ()
		{
			if (GeoPosition.selectedObject != null) {
				Debug.Log ("Adjusting camera angle based on previous selection");
				//If the scene is called from the Marker Info scene
				//then adjust the camera to the position where the scene is left off
				MarkerObject markerObj = GeoPosition.selectedObject;
				Vector2 initVec = new Vector2(1, 1);
				Vector2 posVec = new Vector2 (markerObj.x, markerObj.z);
				float angle = Vector2.Angle (initVec, posVec);

				Debug.Log ("Camera angle of previous object is " + angle);
				Quaternion rot=new Quaternion();
				rot.eulerAngles = new Vector3(0, angle, 0);

				//mainCamera.transform.rotation = rot;
			
				Vector3 selectedObjVec = new Vector3 (markerObj.x, markerObj.y, markerObj.z);


				//Quaternion target = Quaternion.Euler(markerObj.x, 0, markerObj.z);
				//transform.rotation = Quaternion.Lerp (transform.rotation, target, Time.deltaTime * 100.0F);

				transform.LookAt (selectedObjVec);
			}
		}
	}
}
