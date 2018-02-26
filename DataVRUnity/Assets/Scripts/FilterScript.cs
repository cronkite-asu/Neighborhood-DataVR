namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.SceneManagement;
	using System.Collections.Generic;

	public class FilterScript : MonoBehaviour
	{
		public GameObject checkboxTemplate;

		AsyncOperation sceneAO;
		[SerializeField] GameObject loadingUI;
		[SerializeField] Slider loadingProgbar;
		[SerializeField] Text loadingText;
		[SerializeField] GameObject filterCanvas;

		// the actual percentage while scene is fully loaded
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
			Configuration.filterSelection ();
			//SceneManager.LoadScene ("MapScene", LoadSceneMode.Single);
			//sceneAO = SceneManager.LoadSceneAsync("MapScene");
			ChangeScene ("MapScene");

		}

		IEnumerator LoadingSceneRealProgress(string sceneName) {
			yield return new WaitForSeconds(1);
			sceneAO = SceneManager.LoadSceneAsync(sceneName);

			// disable scene activation while loading to prevent auto load
			sceneAO.allowSceneActivation = false;

			while (!sceneAO.isDone) {
				loadingProgbar.value = sceneAO.progress;
				Debug.Log ("Progress " + sceneAO.progress);
				if ((sceneAO.progress ) >= LOAD_READY_PERCENTAGE) {
					//loadingProgbar.value = 1f;
					//loadingText.text = "PRESS SPACE TO CONTINUE";
					//if (Input.GetKeyDown(KeyCode.Space)) {
						sceneAO.allowSceneActivation = true;
					//}
				}
				Debug.Log(sceneAO.progress);
				yield return null;
			}
			//sceneAO.allowSceneActivation = true;
		}


		public void ChangeScene(string sceneName){
			filterCanvas.SetActive (false);
			GameObject canvas = GameObject.Find("Arrows");
			canvas.SetActive (false);
			loadingUI.SetActive(true);
			Debug.Log ("ChangeScene() method called..");
			loadingText.text = "LOADING...";

			StartCoroutine(LoadingSceneRealProgress(sceneName));
		}
	}

}