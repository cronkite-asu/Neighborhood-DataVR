namespace edu.asu.cronkite.datavr
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class CheckBox : MonoBehaviour
	{
		public Text textField;
		public Toggle toogle;
		public Configuration config;

		// Use this for initialization
		void Start ()
		{
			
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void setLabel (string text)
		{
			//GameObject labelGameObj = gameObject.transform.Find ("Label").gameObject;
			//Text labelText = labelGameObj.GetComponent<Text> ();
			//labelText.text = text;
			textField.text = text;
		}

		public void setEnabled (bool enable)
		{
			//Toggle toggleGameObj = this.gameObject.GetComponent<Toggle> ();
			//toggleGameObj.isOn = enable;
			toogle.isOn = true;
			//toogle.onValueChanged() 
			toogle.onValueChanged.AddListener (delegate(bool value) {
				config.filterValueChanged(textField.text, value);
			});
		}
	}
}