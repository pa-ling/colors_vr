using UnityEngine;

public class PaintOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Destroy(gameObject);
	}
}
