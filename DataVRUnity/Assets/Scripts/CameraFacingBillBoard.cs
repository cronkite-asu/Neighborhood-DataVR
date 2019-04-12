using UnityEngine;

public class CameraFacingBillBoard : MonoBehaviour
{
	public Camera m_Camera;

	void Update ()
	{
		transform.LookAt (transform.position + m_Camera.transform.rotation * Vector3.forward,
			m_Camera.transform.rotation * Vector3.up);
	}
}
