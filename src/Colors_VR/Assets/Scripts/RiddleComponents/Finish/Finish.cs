using System.Collections;
using UnityEngine;

public class Finish : MonoBehaviour
{
	public Companion companion;
	public Transform companionPosition;

	public AudioClip[] audioClips;

	private bool finished = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !finished)
		{
			finished = true;
			StartCoroutine(FinishSpeaking());
		}
	}

	private IEnumerator FinishSpeaking()
	{
		float delay = 1.0f;

		companion.SetAutoFollow(false);
		companion.SetIdle(false);

		companion.MoveTo(companionPosition.position);

		yield return new WaitForSeconds(3.0f);

		companion.StartSpeaking(audioClips[0]);
		yield return new WaitForSeconds(audioClips[0].length + delay);

		yield return new WaitForSeconds(7.0f);

		companion.StartSpeaking(audioClips[1]);
		yield return new WaitForSeconds(audioClips[1].length + delay);

		SteamVR_Fade.Start(Color.black, 3.0f);

		yield return null;
	}
}
