namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	using UnityEngine;
	using Mapbox.Unity.MeshGeneration.Data;
	using Mapbox.Unity.MeshGeneration.Components;
	using edu.asu.cronkite.datavr;
	using System;

	/// <summary>
	/// Custom Modifer : This class extends GameObjectModifier
	/// This modifier is added to the merged stack modifier and run method will be run for executed for each
	/// map tile rendered in the screen
	/// </summary>
	[CreateAssetMenu (menuName = "Mapbox/Modifiers/Custom Modifier")]
	public class CustomModifier : GameObjectModifier
	{
		private  GeoPosition markerScript;

		public override void Run (VectorEntity fb, UnityTile tile)
		{
			try {
				// Get the static singleton instance.
				markerScript = GeoPosition.Instance;
				// Perform any operation - here height of the buildings are measured.
				markerScript.performAction ();
			} catch (Exception ex) {
				Debug.LogException (ex);
			}
		}
	}
}
