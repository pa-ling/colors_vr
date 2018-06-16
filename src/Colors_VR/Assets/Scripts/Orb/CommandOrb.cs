using UnityEngine;

public class CommandOrb : Orb
{
	[HideInInspector]
	public Companion companion;

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

        if (!stopCollision)
        {
            companion.MoveTo(collision.contacts[0].point);
        }

		Destroy(gameObject);
	}
}
