using System.Collections;
using UnityEngine;

public class IntroductionRoom : MonoBehaviour
{
	[Header("Doors")]
	public Door firstDoor;
	public Door secondDoor;

	[Header("Orbs")]
	public TakeOrb paintOrb;
	public TakeOrb teleportOrb;

	[Header("Companion")]
	public Companion companion;
	public AudioClip[] audioClips;

	private bool paintOrbWasShot;
	private bool teleportOrbWasShot;
	private bool orbChanged;

	private GameObject player;

	private void Start()
	{
		player = GameObject.Find("[DebugPlayer]");

		if (player == null)
			player = GameObject.Find("[CameraRig]");

		OrbGun[] orbGuns = player.GetComponentsInChildren<OrbGun>(true);

		for (int i = 0; i < orbGuns.Length; ++i)
		{
			orbGuns[i].OnOrbShot += OrbGunShot;
			orbGuns[i].OnChangeOrb += OrbChange;
		}

		paintOrbWasShot = false;
		teleportOrbWasShot = false;
		orbChanged = false;

		paintOrb.SetActive(false);
		teleportOrb.SetActive(false);

		StartCoroutine(Introduction());
	}

	private void OrbGunShot(OrbType orbType)
	{
		if (orbType == OrbType.PaintOrb)
			paintOrbWasShot = true;
		else if (orbType == OrbType.TeleportOrb)
			teleportOrbWasShot = true;
	}

	private void OrbChange(OrbType orbType)
	{
		orbChanged = true;
	}

	private IEnumerator Introduction()
	{
		float delay = 1.0f;

		yield return new WaitForSeconds(5.0f);

		companion.StartSpeaking(audioClips[0]);
		yield return new WaitForSeconds(audioClips[0].length + delay);

		InvokeRepeating("CompanionSayImHere", 5.0f, 5.0f);

		while (!companion.IsVisibleByAnyCamera())
			yield return null;

		CancelInvoke("CompanionSayImHere");

		yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[2]);
		yield return new WaitForSeconds(audioClips[2].length + delay);

		Vector3 lastCameraPosition = Camera.main.transform.position;

		do
		{
			yield return new WaitForSeconds(1.0f);
		} while (lastCameraPosition == Camera.main.transform.position);

		companion.StartSpeaking(audioClips[3]);
		yield return new WaitForSeconds(audioClips[3].length + delay);

		Vector3 orbPosition = companion.transform.position + companion.transform.forward;
		orbPosition.y = 1.5f;
		paintOrb.transform.position = orbPosition;
		paintOrb.SetActive(true);
		while (!paintOrb.taken)
			yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[4]);
		yield return new WaitForSeconds(audioClips[4].length + delay);

		VRPlayerController vrPlayerController = player.GetComponent<VRPlayerController>();
		if (vrPlayerController != null)
			vrPlayerController.StartBlinkingTrigger();

		do
		{
			yield return new WaitForSeconds(1.0f);
		} while (!paintOrbWasShot);

		if (vrPlayerController != null)
			vrPlayerController.StopBlinkTrigger();

		companion.StartSpeaking(audioClips[5]);
		yield return new WaitForSeconds(audioClips[5].length + delay);

		orbPosition = companion.transform.position + companion.transform.forward;
		orbPosition.y = 1.5f;
		teleportOrb.transform.position = orbPosition;
		teleportOrb.SetActive(true);
		while (!teleportOrb.taken)
			yield return new WaitForSeconds(1.0f);

		yield return new WaitForSeconds(1.0f);
		firstDoor.OpenDoor();
		yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[6]);
		yield return new WaitForSeconds(audioClips[6].length + delay);

		companion.SetAutoFollow(true);

		do
		{
			yield return new WaitForSeconds(1.0f);
		} while (!teleportOrbWasShot);

		yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[7]);
		yield return new WaitForSeconds(audioClips[7].length + delay);

		orbChanged = false;

		//if (vrPlayerController != null)
		//	vrPlayerController.StartBlinkingTrackpad();

		do
		{
			yield return new WaitForSeconds(1.0f);
		} while (!orbChanged);

		//if (vrPlayerController != null)
		//	vrPlayerController.StopBlinkTrackpad();

		companion.StartSpeaking(audioClips[8]);
		yield return new WaitForSeconds(audioClips[8].length + delay);

		yield return new WaitForSeconds(1.0f);
		secondDoor.OpenDoor();
		yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[9]);
		yield return null;
	}

	private void CompanionSayImHere()
	{
		companion.StartSpeaking(audioClips[1]);
	}
}
