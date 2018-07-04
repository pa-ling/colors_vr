using System.Collections;
using UnityEngine;

public class TeleportOrb : Orb
{
	[HideInInspector]
	public Transform playerTransform;
	public LayerMask ableToTeleportOnLayer;

	private AudioSource audioSource;

	protected override void Start()
	{
		base.Start();

		audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision collision)
    {
		uint bitstring = (uint)ableToTeleportOnLayer.value;
		for (int i = 31; bitstring > i; --i)
		{
			if ((bitstring >> i) > 0)
			{
				bitstring = ((bitstring << 32 - i) >> 32 - i);
				if (collision.collider.gameObject.layer != i)
					return;
			}
		}

		if (collision.contacts[0].normal != collision.gameObject.transform.up)
			return;

		if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
			return;

		Splat(collision);

		AudioSource.PlayClipAtPoint(splashSound, transform.position, 35.0f);

		StartCoroutine(Teleport(new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), 0.2f));

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        meshRenderer.enabled = false;
	}

	public IEnumerator Teleport(Vector3 targetPosition, float targetTime)
	{
        audioSource.Play();

        SteamVR_Fade.Start(Color.black, targetTime / 2.0f);
		yield return new WaitForSeconds(targetTime / 2.0f);

		playerTransform.position = targetPosition;

        SteamVR_Fade.Start(Color.clear, targetTime / 2.0f);
		yield return new WaitForSeconds(targetTime / 2.0f);

        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
