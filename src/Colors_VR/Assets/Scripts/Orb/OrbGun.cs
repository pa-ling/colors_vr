using UnityEngine;

public class OrbGun : MonoBehaviour
{
	public LayerMask ignoreLayer;
	public Transform playerRoor;
	public ParticleSystem dropletParticleSystem;
	public GameObject commandOrb = null;
	public GameObject paintOrb = null;
	public GameObject physicsOrb = null;
	public GameObject teleportOrb = null;

	private OrbType currentOrb = OrbType.PaintOrb;
	private CommandOrb commandOrbComponent;
	private PaintOrb paintOrbComponent;
	private PhysicsOrb physicsOrbComponent;
	private TeleportOrb teleportOrbComponent;

	private void Start()
	{
		int orbLayer = LayerMask.NameToLayer("Orb");
		uint bitstring = (uint)ignoreLayer.value;
		for (int i = 31; bitstring > i; --i)
		{
			if ((bitstring >> i) > 0)
			{
				bitstring = ((bitstring << 32 - i) >> 32 - i);
				Physics.IgnoreLayerCollision(orbLayer, i);
			}
		}

		commandOrbComponent = commandOrb.GetComponent<CommandOrb>();
		paintOrbComponent = paintOrb.GetComponent<PaintOrb>();
		physicsOrbComponent = physicsOrb.GetComponent<PhysicsOrb>();
		teleportOrbComponent = teleportOrb.GetComponent<TeleportOrb>();

		commandOrb.GetComponent<CollisionDetector>().collidingLayer = ~ignoreLayer;
		paintOrb.GetComponent<CollisionDetector>().collidingLayer = ~ignoreLayer;
		physicsOrb.GetComponent<CollisionDetector>().collidingLayer = ~ignoreLayer;
		teleportOrb.GetComponent<CollisionDetector>().collidingLayer = ~ignoreLayer;

		teleportOrbComponent.playerTransform = playerRoor;
		teleportOrbComponent.playerTransform = playerRoor;
		teleportOrbComponent.playerTransform = playerRoor;
		teleportOrbComponent.playerTransform = playerRoor;
	}

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

		if (currentOrb == OrbType.CommandOrb)
		{
			orbPrefab = commandOrb;
			orbSpeed = commandOrbComponent.speed;
		}
		else if (currentOrb == OrbType.PaintOrb)
		{
			orbPrefab = paintOrb;
			orbSpeed = paintOrbComponent.speed;
		}
		else if (currentOrb == OrbType.PhysicsOrb)
		{
			orbPrefab = physicsOrb;
			orbSpeed = physicsOrbComponent.speed;
		}
		else if (currentOrb == OrbType.TeleportOrb)
		{
			orbPrefab = teleportOrb;
			orbSpeed = teleportOrbComponent.speed;
		}

		GameObject orb = Instantiate(orbPrefab, transform.position, transform.rotation);
		Rigidbody orbsRigidbody = orb.GetComponent<Rigidbody>();

		orbsRigidbody.AddForce(transform.forward * orbSpeed);
	}
}
