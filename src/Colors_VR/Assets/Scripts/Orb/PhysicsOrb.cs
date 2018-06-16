using UnityEngine;

public class PhysicsOrb : Orb
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
