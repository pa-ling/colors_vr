using UnityEngine;

public class CommandOrb : Orb
{
	[HideInInspector]
	public Companion companion;

    protected override void OnCollisionEnter(Collision collision)
	{
		if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
		{
			Destroy(gameObject);
			return;
		}

		base.OnCollisionEnter(collision);

		companion.MoveTo(collision.contacts[0].point);

		Destroy(gameObject);
	}
}
