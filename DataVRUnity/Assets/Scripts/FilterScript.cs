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
			SceneManager.LoadScene ("MapScene", LoadSceneMode.Single);
		}
	}

}