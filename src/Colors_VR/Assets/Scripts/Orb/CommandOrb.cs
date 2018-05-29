using UnityEngine;

public class CommandOrb : Orb
{
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		Debug.Log("Command hit");

		Destroy(gameObject);
	}
}
