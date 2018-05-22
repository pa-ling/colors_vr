using UnityEngine;

public class TeleportOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Debug.Log("TeleportOrb hit");

		Destroy(gameObject);
	}
}
