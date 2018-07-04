using UnityEngine;

public class PaintOrb : Orb
{
	private void OnCollisionEnter(Collision collision)
	{
		if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
			return;

		Splat(collision);

		AudioSource.PlayClipAtPoint(splashSound, transform.position, 35.0f);

		Destroy(gameObject);
    }
}
