namespace edu.asu.cronkite.datavr
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.SceneManagement;

	public class FilterScript : MonoBehaviour
	{
		public GameObject checkboxTemplate;

		AsyncOperation sceneAO;
		[SerializeField] GameObject loadingUI;
		[SerializeField] Slider loadingProgbar;
		[SerializeField] Text loadingText;
		[SerializeField] GameObject filterCanvas;

		// The actual percentage while scene is fully loaded.
		private const float LOAD_READY_PERCENTAGE = 0.9f;

		public void addCheckBoxesForTypes(SortedDictionary<string, bool> filterDict)
		{
			GameObject canvas = GameObject.Find("Canvas");
			foreach(KeyValuePair<string, bool> entry in filterDict)
			{
				addCheckBox(canvas, entry.Key, entry.Value);
			}
		}
	
		private void addCheckBox(GameObject canvas, string label, bool enable)
		{
			GameObject checkBoxGameObj = Instantiate (checkboxTemplate);
			checkBoxGameObj.transform.SetParent(checkboxTemplate.transform.parent, false);
			checkBoxGameObj.SetActive (true);
			CheckBox checkBox = checkBoxGameObj.GetComponent<CheckBox>();
			checkBox.setLabel (label);
			checkBox.setEnabled (enable);			
		}


		/// <summary>
		/// Filters the button clicked.
		/// </summary>
		public void filterButtonClicked()
		{
			Configuration.filterSelection();
			Debug.Log ("Button has been clikced!");
			ChangeScene ("MapScene");
		}

		IEnumerator LoadingSceneRealProgress(string sceneName) {
			yield return new WaitForSeconds(1);
			sceneAO = SceneManager.LoadSceneAsync(sceneName);

			// Disable scene activation while loading to prevent auto load.
			sceneAO.allowSceneActivation = false;

			while (!sceneAO.isDone) {
				loadingProgbar.value = sceneAO.progress;
				// Debug.Log ("Progress " + sceneAO.progress);
				if ((sceneAO.progress ) >= LOAD_READY_PERCENTAGE) {
					sceneAO.allowSceneActivation = true;
				}
				// Debug.Log(sceneAO.progress);
				yield return null;
			}
		}

		public void ChangeScene(string sceneName){
			filterCanvas.SetActive (false);
			GameObject canvas = GameObject.Find("Arrows");
			canvas.SetActive (false);
			loadingUI.SetActive(true);
			// Debug.Log ("ChangeScene() method called..");
			loadingText.text = "LOADING...";

			StartCoroutine(LoadingSceneRealProgress(sceneName));
		}
	}
}