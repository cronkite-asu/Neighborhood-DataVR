namespace Mapbox.Unity.MeshGeneration.Modifiers
{	
	using Mapbox.Unity.MeshGeneration.Data;
	using UnityEngine;
	using Mapbox.Unity.MeshGeneration.Components;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;



	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Custom Modifier")]
	public class CusotmModifier1 : GameObjectModifier
	{
		public static int COUNTER = 1;

		public override void Run(FeatureBehaviour fb, UnityTile tile)
		{
			Debug.Log ("Custom Modified called ...");
			Debug.Log ("Counter value = " + COUNTER);

			//if (COUNTER == 1)
			{
				castRayToGround (new Vector3 (0f, 0.0f, 0.8f));
				COUNTER = COUNTER + 1;
			}
		}

		void castRayToGround(Vector3 position)
		{
			RaycastHit hit;
			float distance = 100f;
			position.y = 50;
			Vector3 targetLocation;


			if (Physics.Raycast(position, Vector3.down, out hit, distance)) {
				targetLocation = hit.point;
				//float height = targetLocation.y;

				Debug.DrawLine (position, hit.point, Color.red, 100);
				Debug.Log ("Ray cast hit position " + hit.point);
			}
		}
}
}
