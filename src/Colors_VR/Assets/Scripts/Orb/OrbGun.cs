using UnityEngine;

public class OrbGun : MonoBehaviour
{
	public GameObject fluidOrb = null;
	public GameObject paintOrb = null;
	public GameObject physicsOrb = null;
	public GameObject teleportOrb = null;

	private OrbType currentOrb = OrbType.PaintOrb;

	public OrbType GetCurrentOrb()
	{
		return currentOrb;
	}

	public void SetCurrentOrbTo(OrbType orbType)
	{
		currentOrb = orbType;
	}

	public void Fire()
	{
		GameObject orbPrefab = null;
		float orbSpeed = 0.0f;

		if (currentOrb == OrbType.FluidOrb)
		{
			orbPrefab = fluidOrb;
			orbSpeed = 500.0f;
		}
		else if (currentOrb == OrbType.PaintOrb)
		{
			orbPrefab = paintOrb;
			orbSpeed = 500.0f;
		}
		else if (currentOrb == OrbType.PhysicsOrb)
		{
			orbPrefab = physicsOrb;
			orbSpeed = 500.0f;
		}
		else if (currentOrb == OrbType.TeleportOrb)
		{
			orbPrefab = teleportOrb;
			orbSpeed = 500.0f;
		}

		GameObject orb = Instantiate(orbPrefab, transform.position, transform.rotation);
		Rigidbody orbsRigidbody = orb.GetComponent<Rigidbody>();

		orbsRigidbody.AddForce(transform.forward * orbSpeed);
	}
}
