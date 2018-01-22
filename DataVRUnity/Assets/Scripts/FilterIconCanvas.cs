using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FilterIconCanvas : MonoBehaviour {

	public void onFilterIconClicked()
	{
		SceneManager.LoadScene ("FilterScene", LoadSceneMode.Single);
	}
}
