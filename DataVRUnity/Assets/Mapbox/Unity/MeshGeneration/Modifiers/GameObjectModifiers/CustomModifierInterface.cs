namespace Mapbox.Unity.MeshGeneration.Modifiers
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	[System.Serializable]
	public class CustomModifierInterface {
		
		// Virtual method 
		public virtual void performAction()
		{
			//Implement the required code in the derived class
		}

}
}