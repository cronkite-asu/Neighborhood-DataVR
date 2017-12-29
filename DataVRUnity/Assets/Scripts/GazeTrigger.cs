using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeTrigger : MonoBehaviour {

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 3f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;

	// Use this for initialization
	void Start () {
		Debug.Log	("GazeTrigger - start() method called...\n");
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log ("GazeTrigger - isLookedAt = " + isLookedAt + "\n");
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
			}
		}

		// Not gazing at this anymore, reset everything.
		else {
			lookTimer = 0f;
			// Reset progress indicator.
			//gazeTimer.GetComponent<Renderer>().material.SetFloat("_Cutoff", 0f);
		}
	}

	public void SetGazedAt(bool gazedAt) {
		Debug.Log ("GazeTrigger - event method i/p arg - " + gazedAt);
		isLookedAt = gazedAt;
	}
}
