namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	using Mapbox.Unity.MeshGeneration.Data;
	using UnityEngine;
	using Mapbox.Unity.MeshGeneration.Components;
	using UnityEngine;
	using edu.asu.cronkite.datavr;

	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Custom Modifier")]
	public class CustomModifier : GameObjectModifier
	{
		private  GeoPosition markerScript;
	
		public override void Run (FeatureBehaviour fb, UnityTile tile)
		{

				markerScript = GeoPosition.Instance;
				Debug.Log ("Name in custom modifier =  " + markerScript.name);

				Debug.Log ("Custom Modified called ...");
				markerScript.performAction ();
				Debug.Log ("custom modifier finished");
		}

	}
}
