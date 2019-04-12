namespace edu.asu.cronkite.datavr
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class FilterIconCanvas : MonoBehaviour
	{
		public void onFilterIconClicked ()
		{
			GeoPosition.updateCounter (0);	
			SceneManager.LoadScene ("FilterScene", LoadSceneMode.Single);
		}
	}
}
