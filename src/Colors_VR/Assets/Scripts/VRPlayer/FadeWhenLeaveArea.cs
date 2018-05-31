using UnityEngine;

public class FadeWhenLeaveArea : MonoBehaviour
{
	public float fadeToBlackTime = 1.0f;
	public float fadeToClearTime = 0.1f;

	private VRPlayerController vrPlayerController;

	private void Start()
	{
		vrPlayerController = GetComponentInParent<VRPlayerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		SteamVR_Fade.Start(Color.black, fadeToBlackTime);
	}

	private void OnTriggerStay(Collider other)
	{
		vrPlayerController.Vibration(Time.deltaTime);
	}

	private void OnTriggerExit(Collider other)
	{
		SteamVR_Fade.Start(Color.clear, fadeToClearTime);
	}
}
