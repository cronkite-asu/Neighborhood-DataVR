namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.SceneManagement;

	public class FilterScript : MonoBehaviour
	{
		public GameObject checkboxTemplate;


		// Use this for initialization
		void Start ()
		{
			//GeoPosition.Instance;
		
		/*

			//CheckBox checkbox1 = canvas.AddComponent<CheckBox> ();
				GameObject checkBoxGameObj = (GameObject)Instantiate (Resources.Load ("CheckBox"), canvas.transform);
			//Transform checkboxTransform = (Transform)checkbox1.GetComponent<Transform> ();
			//checkBox.transform.parent = canvas.transform;
			CheckBox checkBox = checkBoxGameObj.GetComponent<CheckBox>();
			checkBox.setLabel ("Type name");
			checkBox.setEnabled (true);
			*/
		
		}

		public void addCheckBoxesForTypes(HashSet<string> typeSet)
		{
			GameObject canvas = GameObject.Find("Canvas");
			foreach(string type in typeSet)
			{
				addCheckBox(canvas, type, true);
			}
		}
	
		private void addCheckBox(GameObject canvas, string label, bool enable)
		{
			//GameObject checkBoxGameObj = (GameObject)Instantiate (Resources.Load ("CheckBox"), canvas.transform);
			GameObject checkBoxGameObj = Instantiate (checkboxTemplate);
			//Transform checkboxTransform = (Transform)checkbox1.GetComponent<Transform> ();
			//checkBox.transform.setParent( = canvas.transform;
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

		// Update is called once per frame
		void Update ()
		{
		
		}


		public void addFiltersToCanvas()
		{	
			//dynamically add the checkboxes to the UI
		}

		public void filterByType()
		{
			//Fetch the values from the checkboxes and then filter the data based on the selection
			//Change to map scene
		}
	}

}