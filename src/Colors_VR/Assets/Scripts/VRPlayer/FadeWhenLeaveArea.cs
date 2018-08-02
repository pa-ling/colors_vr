using UnityEngine;

/*
Fades to black and lets Controller vibrate when player walks in gameobject
*/
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
		if (other.isTrigger)
			return;

		Color fadeBlack = new Color(0.0f, 0.0f, 0.0f, 0.99f);
		SteamVR_Fade.Start(fadeBlack, fadeToBlackTime);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.isTrigger)
			return;

		vrPlayerController.Vibration(ViveController.BOTH, Time.deltaTime);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger)
			return;

		SteamVR_Fade.Start(Color.clear, fadeToClearTime);
	}
}
