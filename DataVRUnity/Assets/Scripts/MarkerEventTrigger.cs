namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.SceneManagement;

	public class MarkerEventTrigger : EventTrigger
	{
		//Marker object that this script is attached to.
		private MarkerObject markerObj;

		private bool isLookedAt = false;

		// How long the user can gaze at this before the button is clicked.
		public float timerDuration = 3f;

		// Count time the player has been gazing at the button.
		private float lookTimer = 0f;


		void Update () {

			// While player is looking at this button.
			if (isLookedAt) {

				// Increment the gaze timer.
				lookTimer += Time.deltaTime;

				// Gaze time exceeded limit - button is considered clicked.
				if (lookTimer > timerDuration) {
					lookTimer = 0f; // reset the timer to zero for the next gaze
					// Show marker information
					showMarkerInfo();
				}
			}

			// Not gazing at this anymore, reset everything.
			else {
				lookTimer = 0f;
				// Reset progress indicator.
			}
		}



		public void setMarkerObj(MarkerObject markerObj)
		{
			this.markerObj = markerObj;
		}


		public override void OnPointerClick (PointerEventData data)
		{
			showMarkerInfo ();
		}

		public override void OnPointerEnter (PointerEventData data)
		{
			SetGazedAt (true);
			ShowGazeTimer (true);
		}

		public override void OnPointerExit (PointerEventData data)
		{
			SetGazedAt (false);
			ShowGazeTimer (false);
		}
			
		private void showMarkerInfo()
		{
			MakerInfoScript.showMarkerDetails(this.markerObj);
		}

		//Method which sets the isLookedAt attribute for deciding whether the 
		private void SetGazedAt(bool gazedAt) {
			isLookedAt = gazedAt;
		}

		/// <summary>
		/// Shows the gaze timer.
		/// - method to show the gaze timer or not
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		private void ShowGazeTimer(bool show)
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("MainCamera");
			GameObject timerCanvas = obj.transform.Find("TimerGazeCanvas").gameObject;

			//Show radial progress bar when the recticle pointer enters the game obejct 
			if (show) {
				timerCanvas.SetActive (true);
				GameObject rpb = GameObject.Find("RadialProgressBar");
				RPB script = rpb.GetComponent<RPB> ();
				script.currentAmount = 0;
			} else {
				//Hide the timer when the recticle pointer is moved out of marker object
				timerCanvas.SetActive (false);
			}
		}
	}
}
