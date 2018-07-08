using System.Collections;
using UnityEngine;

public class TeleportPuzzle1 : MonoBehaviour
{
	[Header("Door")]
	public Door door;

	[Header("PressButton")]
	public PressButton pressButton;

	[Header("Orb")]
	public TakeOrb physicsOrb;

	[Header("Companion")]
	public Companion companion;
	public AudioClip[] audioClips;

	private bool redOrbIntroductionStarted;
	private Transform companionPosition;

    private void Start ()
	{
        pressButton.OnPressButtonHit += door.OpenDoor;

		redOrbIntroductionStarted = false;

		companionPosition = transform.Find("CompanionPosition");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9)
		{
			if (!redOrbIntroductionStarted)
			{
				redOrbIntroductionStarted = true;
				companion.SetAutoFollow(false);
				companion.MoveTo(companionPosition.position);
				StartCoroutine(Introduction());
			}
		}
	}

	private IEnumerator Introduction()
	{
		float delay = 1.0f;

		yield return new WaitForSeconds(1.0f);

		while (companion.IsMoving())
			yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[0]);
		yield return new WaitForSeconds(audioClips[0].length + delay);

		Vector3 orbPosition = companion.transform.position + companion.transform.forward;
		orbPosition.y = 22.5f;
		physicsOrb.transform.position = orbPosition;
		physicsOrb.SetActive(true);
		while (!physicsOrb.taken)
			yield return new WaitForSeconds(1.0f);

		companion.StartSpeaking(audioClips[1]);
		companion.SetAutoFollow(true);

		yield return null;
	}
}
