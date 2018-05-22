using UnityEngine;

public class PhysicsOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Debug.Log("PhysicsOrb hit");

		Destroy(gameObject);
	}
}
