using UnityEngine;

public class PaintOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Debug.Log("PaintOrb hit");

		Destroy(gameObject);
	}
}
