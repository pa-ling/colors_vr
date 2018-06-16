using UnityEngine;

public class PaintOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

        if (!stopCollision)
        {
            AudioSource.PlayClipAtPoint(splashSound, transform.position);
        }

        Destroy(gameObject);
    }
}
