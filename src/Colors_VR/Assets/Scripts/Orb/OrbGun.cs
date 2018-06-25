using UnityEngine;

public class OrbGun : MonoBehaviour
{
	[Header("Layer Masks")]
	public LayerMask dontCollidWith;
	public LayerMask dontLeaveSplatsOn;
	[Header("Orb Prefabs")]
	public GameObject commandOrb = null;
	public GameObject paintOrb = null;
	public GameObject physicsOrb = null;
	public GameObject teleportOrb = null;
	[Header("Transform for Teleport")]
	public Transform playerRoor;
	[Header("Companion")]
	public Companion companion;

	private MeshRenderer meshRenderer;
	private AudioSource audioSource;

	private OrbType currentOrb/* = OrbType.PaintOrb*/;
	private CommandOrb commandOrbComponent;
	private PaintOrb paintOrbComponent;
	private PhysicsOrb physicsOrbComponent;
	private TeleportOrb teleportOrbComponent;

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

		meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material.color = paintOrb.GetComponent<MeshRenderer>().sharedMaterial.color;

        audioSource = GetComponent<AudioSource>();

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
	}

	public OrbType GetCurrentOrb()
	{
		return currentOrb;
	}

	public void SetCurrentOrbTo(OrbType orbType)
	{
		currentOrb = orbType;

        if (currentOrb == OrbType.CommandOrb)
            meshRenderer.material.color = commandOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
        else if (currentOrb == OrbType.PaintOrb)
            meshRenderer.material.color = paintOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
        else if (currentOrb == OrbType.PhysicsOrb)
            meshRenderer.material.color = physicsOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
        else if (currentOrb == OrbType.TeleportOrb)
            meshRenderer.material.color = teleportOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
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
