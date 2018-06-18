using UnityEngine;

public class OrbGun : MonoBehaviour
{
	public LayerMask dontCollidWith;
	public LayerMask dontLeaveSplatsOn;
	public Transform playerRoor;
	public GameObject commandOrb = null;
	public GameObject paintOrb = null;
	public GameObject physicsOrb = null;
	public GameObject teleportOrb = null;
	public Companion companion;

	private OrbType currentOrb = OrbType.PaintOrb;
	private CommandOrb commandOrbComponent;
	private PaintOrb paintOrbComponent;
	private PhysicsOrb physicsOrbComponent;
	private TeleportOrb teleportOrbComponent;

    private AudioSource audioSource;

	private void Start()
	{
		int orbLayer = LayerMask.NameToLayer("Orb");
		uint bitstring = (uint)dontCollidWith.value;
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

		commandOrb.GetComponent<CollisionDetector>().collidingLayer = ~dontCollidWith;
		paintOrb.GetComponent<CollisionDetector>().collidingLayer = ~dontCollidWith;
		physicsOrb.GetComponent<CollisionDetector>().collidingLayer = ~dontCollidWith;
		teleportOrb.GetComponent<CollisionDetector>().collidingLayer = ~dontCollidWith;

		commandOrbComponent.dontLeaveSplatsOn = dontLeaveSplatsOn;
		commandOrbComponent.companion = companion;
		paintOrbComponent.dontLeaveSplatsOn = dontLeaveSplatsOn;
		physicsOrbComponent.dontLeaveSplatsOn = dontLeaveSplatsOn;
		teleportOrbComponent.dontLeaveSplatsOn = dontLeaveSplatsOn;
		teleportOrbComponent.playerTransform = playerRoor;

        audioSource = GetComponent<AudioSource>();
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

        audioSource.Play();
	}
}
