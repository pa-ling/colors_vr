using System.Collections;
using UnityEngine;

public class TeleportOrb : Orb
{
	[HideInInspector]
	public Transform playerTransform;
	public LayerMask ableToTeleportOnLayer;

    protected override void OnCollisionEnter(Collision collision)
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

		base.OnCollisionEnter(collision);

		StartCoroutine(Teleport(new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), 0.2f));

		meshRenderer.enabled = false;
	}

	private IEnumerator Teleport(Vector3 targetPosition, float targetTime)
	{
		SteamVR_Fade.Start(Color.black, 0.1f);
		yield return new WaitForSeconds(0.1f);

		playerTransform.position = targetPosition;

		SteamVR_Fade.Start(Color.clear, 0.1f);
		yield return new WaitForSeconds(0.1f);

		Destroy(gameObject);
	}
}
