using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPB : MonoBehaviour
{
	public Transform LoadingBar;
	public Transform TextIndicator;

	[SerializeField] public float currentAmount;
	[SerializeField] private float speed;

	//Call on each frame update - updates the gaze timer when the user is looking on the marker objects
	void Update ()
	{
		if (currentAmount < 100) {
			currentAmount += speed * Time.deltaTime;
			//Uncomment to show the seconds remaining.
			//TextIndicator.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "s";
		} else {
			
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100;
	}
}
