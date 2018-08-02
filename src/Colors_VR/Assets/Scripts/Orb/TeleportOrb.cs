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
					return;                                                                                     //bounces off all layers except "Teleportable"
			}
		}

		if (collision.contacts[0].normal != collision.gameObject.transform.up)                                  //if it doesn't collide with the top side of an object it bounces off
			return;

		if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)           //if it collides with "dontLeaveSplatsOn" - layer it bounces off
            return;

		Splat(collision);

		AudioSource.PlayClipAtPoint(splashSound, transform.position, 35.0f);

		Vector3 targetPosition = collision.contacts[0].point;
		//Debug.Log("Old targetPosition: " + targetPosition);
		//RaycastHit raycastHit;
		//Debug.DrawRay(targetPosition, Vector3.up, Color.red);
		////if (Physics.BoxCast(targetPosition, Vector3.one/*new Vector3(0.5f, 0.1f, 0.5f)*/, Vector3.zero, out raycastHit))
		//if (Physics.BoxCast(targetPosition + Vector3.up, new Vector3(10.0f, 0.01f, 10.0f), Vector3.up, out raycastHit, Quaternion.identity, 0.1f))
		//{
		//	Debug.Log("Something in the way. " + raycastHit.point + ", " + raycastHit.normal);
		//	Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red);

		//	targetPosition += raycastHit.normal * 0.25f;

		//	Debug.Log("New targetPosition: " + targetPosition);
		//}
		//else
		//	Debug.Log("Nothing in the way.");

		StartCoroutine(Teleport(targetPosition, 0.2f));

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
