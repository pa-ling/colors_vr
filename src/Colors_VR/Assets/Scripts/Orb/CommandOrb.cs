using UnityEngine;

public class CommandOrb : Orb
{
	[HideInInspector]
	public Companion companion;

    private void OnCollisionEnter(Collision collision)
	{
		if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)               //if it collides with "dontLeaveSplatsOn"-layer it bounces off
            return;

		Splat(collision);

		AudioSource.PlayClipAtPoint(splashSound, transform.position, 35.0f);

		companion.MoveTo(collision.contacts[0].point);

		Destroy(gameObject);
	}
}
