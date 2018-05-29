using UnityEngine;

public class FadeWhenLeaveArea : MonoBehaviour
{
	public float fadeToBlackTime = 1.0f;
	public float fadeToClearTime = 0.1f;

	private void OnTriggerEnter(Collider other)
	{
		SteamVR_Fade.Start(Color.black, fadeToBlackTime);
	}

	private void OnTriggerExit(Collider other)
	{
		SteamVR_Fade.Start(Color.clear, fadeToClearTime);
	}
}
