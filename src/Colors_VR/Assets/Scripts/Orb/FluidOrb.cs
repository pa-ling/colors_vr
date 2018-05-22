using UnityEngine;

public class FluidOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Debug.Log("FluidOrb hit");

		Destroy(gameObject);
	}
}
