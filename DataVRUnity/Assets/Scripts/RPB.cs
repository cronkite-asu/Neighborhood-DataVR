namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using System;

	public class RPB : MonoBehaviour
	{
		public Transform LoadingBar;
		public Transform TextIndicator;

		/*[SerializeField]*/
		private float currentAmount;
		/*[SerializeField]*/
		private float speed;
		private float GazeTime = 0f;
		public float GazeTimeLimit;

		public void Start ()
		{
			LoadingBar.GetComponent<Image> ().fillAmount = 0;
			GazeTimeLimit = GeoPosition.Instance.getGazeTimeLimit ();
		}

		//Call on each frame update - updates the gaze timer when the user is looking on the marker objects
		/*void Update ()
		{
			GazeTime += Time.deltaTime;
			LoadingBar.GetComponent<Image> ().fillAmount = GazeTime / GazeTimeLimit;
		}*/


		public void SetLoadingBarFillProgress(float progress)
		{
		LoadingBar.GetComponent<Image> ().fillAmount = progress;
		}

		public void Reset ()
		{
			LoadingBar.GetComponent<Image> ().fillAmount = 0;
		}
	}
}