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

		}

		public override void OnPointerExit (PointerEventData data)
		{
			Debug.Log ("OnPointerExit called.");
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
			SceneManager.LoadScene("MarkerInfo");
			GeoPosition.selectedObject = this.markerObj;
		}

	}
}
