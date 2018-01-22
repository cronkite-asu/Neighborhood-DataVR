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

		public void setLabel (string text)
		{
			textField.text = text;
		}

		public void setEnabled (bool enable)
		{
			toogle.isOn = enable;
			toogle.onValueChanged.AddListener (delegate(bool value) {
				config.filterValueChanged (textField.text, value);
			});
		}
	}
}