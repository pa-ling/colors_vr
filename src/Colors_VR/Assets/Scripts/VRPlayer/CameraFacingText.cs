using UnityEngine;

public class CameraFacingText : MonoBehaviour
{
	public Camera cameraToLookAt;

	private void Update()
	{
		Vector3 lookAtDirection = cameraToLookAt.transform.position - transform.position;

		lookAtDirection.x = lookAtDirection.z = 0.0f;

		transform.LookAt(cameraToLookAt.transform.position - lookAtDirection);
		transform.Rotate(0.0f, 180.0f, 0.0f);
	}
}
