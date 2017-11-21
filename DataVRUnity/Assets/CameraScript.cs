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
				//If the scene is called from the Marker Info scene
				//then adjust the camera to the position where the scene is left off
				MarkerObject markerObj = GeoPosition.selectedObject;
				Vector2 initVec = new Vector2(0, 0);
				Vector2 posVec = new Vector2 (markerObj.x, markerObj.z);
				float angle = Vector2.Angle (initVec, posVec);

				Quaternion rot=new Quaternion();
				rot.eulerAngles = new Vector3(0, angle, 0);

				GameObject mainCamera= GameObject.Find("Main Camera");
				mainCamera.transform.rotation = rot;
			}
		}
	}
}
