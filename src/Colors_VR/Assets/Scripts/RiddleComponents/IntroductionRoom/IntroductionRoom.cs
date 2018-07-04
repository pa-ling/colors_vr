using UnityEngine;
using System.Collections;

public class IntroductionRoom : MonoBehaviour
{
	public Companion companion;
	public Door door;
	public TakeOrb paintOrb;
	public TakeOrb teleportOrb;

	public AudioClip[] audioClips;

	private void Start()
	{
		paintOrb.SetActive(false);
		teleportOrb.SetActive(false);

		StartCoroutine(Introduction());
	}

	private IEnumerator Introduction()
	{
		yield return new WaitForSeconds(5.0f);

		companion.StartSpeaking(audioClips[0]);

		yield return new WaitForSeconds(5.0f);

		paintOrb.SetActive(true);
		while (!paintOrb.taken)
			yield return null;

		yield return new WaitForSeconds(5.0f);

		teleportOrb.SetActive(true);
		while (!teleportOrb.taken)
			yield return null;

		yield return new WaitForSeconds(3.0f);

		door.OpenDoor();
	}
}
