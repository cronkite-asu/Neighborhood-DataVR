namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.SceneManagement;

	public class MarkerEventTrigger : EventTrigger
	{
		private MarkerObject markerObj;

		private bool isLookedAt = false;

		// How long the user can gaze at this before the button is clicked.
		public float timerDuration = 3f;

		// Count time the player has been gazing at the button.
		private float lookTimer = 0f;


		void Update () {

			//Debug.Log ("GazeTrigger - isLookedAt = " + isLookedAt + "\n");
			// While player is looking at this button.
			if (isLookedAt) {

				// Increment the gaze timer.
				lookTimer += Time.deltaTime;

				// Modify graphic progress indicator to show remaining time. E.g. set the alpha layer value
				// cutoff on a PNG so the part showing is proportional to remaining time.
				//gazeTimer.GetComponent<Renderer>().material.SetFloat("_Cutoff", lookTimer / timerDuration);

				// Gaze time exceeded limit - button is considered clicked.
				if (lookTimer > timerDuration) {
					lookTimer = 0f;

					Debug.Log("TIME COMPLTED - DO ACTION Button selected!");
					//GetComponent<Button>().onClick.Invoke();
					changeToMarkerInfoScene();
				}
			}

			// Not gazing at this anymore, reset everything.
			else {
				lookTimer = 0f;
				// Reset progress indicator.
				//gazeTimer.GetComponent<Renderer>().material.SetFloat("_Cutoff", 0f);
			}
		}



		public void setMarkerObj(MarkerObject markerObj)
		{
			this.markerObj = markerObj;
		}

		public override void OnBeginDrag (PointerEventData data)
		{
			Debug.Log ("OnBeginDrag called.");
		}

		public override void OnCancel (BaseEventData data)
		{
			Debug.Log ("OnCancel called.");
		}

		public override void OnDeselect (BaseEventData data)
		{
			Debug.Log ("OnDeselect called.");
		}

		public override void OnDrag (PointerEventData data)
		{
			Debug.Log ("OnDrag called.");
		}

		public override void OnDrop (PointerEventData data)
		{
			Debug.Log ("OnDrop called.");
		}

		public override void OnEndDrag (PointerEventData data)
		{
			Debug.Log ("OnEndDrag called.");
		}

		public override void OnInitializePotentialDrag (PointerEventData data)
		{
			Debug.Log ("OnInitializePotentialDrag called.");
		}

		public override void OnMove (AxisEventData data)
		{
			Debug.Log ("OnMove called.");
		}

		public override void OnPointerClick (PointerEventData data)
		{
			Debug.Log ("OnPointerClick called.");
			changeToMarkerInfoScene ();
		}

		public override void OnPointerDown (PointerEventData data)
		{
			Debug.Log ("OnPointerDown called.");
		}

		public override void OnPointerEnter (PointerEventData data)
		{
			Debug.Log ("OnPointerEnter called.");
			SetGazedAt (true);
			ShowGazeTimer (true);
		}

		public override void OnPointerExit (PointerEventData data)
		{
			Debug.Log ("OnPointerExit called.");
			SetGazedAt (false);
			ShowGazeTimer (false);
		}

		public override void OnPointerUp (PointerEventData data)
		{
			Debug.Log ("OnPointerUp called.");
		}

		public override void OnScroll (PointerEventData data)
		{
			Debug.Log ("OnScroll called.");
		}
	
		public override void OnSelect (BaseEventData data)
		{
			Debug.Log ("OnSelect called.");
		}

		public override void OnSubmit (BaseEventData data)
		{
			Debug.Log ("OnSubmit called.");
		}

		public override void OnUpdateSelected (BaseEventData data)
		{
			Debug.Log ("OnUpdateSelected called.");
		}

		private void changeToMarkerInfoScene()
		{

		//TODO: find a way to cache the loaded scene coordinate from the rectile pointer
			Debug.Log("Showing the details of the maker : " +this.name);
			//SceneManager.LoadScene("MarkerInfo");
			//GeoPosition.selectedObject = this.markerObj;
			MakerInfoScript.showMarkerDetails(this.markerObj);
		}

		private void SetGazedAt(bool gazedAt) {
			Debug.Log ("GazeTrigger - event method i/p arg - " + gazedAt);
			isLookedAt = gazedAt;
		}

		private void ShowGazeTimer(bool show)
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("MainCamera");
			GameObject timerCanvas = obj.transform.Find("TimerGazeCanvas").gameObject;
			if (show) {
				//Canvas[] canvas = obj.GetComponentsInChildren(Canvas, true);
				timerCanvas.SetActive (true);
				GameObject rpb = GameObject.Find("RadialProgressBar");
				RPB script = rpb.GetComponent<RPB> ();
				script.currentAmount = 0;
			} else {
				timerCanvas.SetActive (false);
			}
		}
	}
}
