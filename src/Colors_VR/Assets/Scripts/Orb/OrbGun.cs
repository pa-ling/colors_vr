using System;
using UnityEngine;

public class OrbGun : MonoBehaviour
{
	[Header("Layer Masks")]
	public LayerMask dontCollidWith;
	public LayerMask dontLeaveSplatsOn;
	[Header("Orb Prefabs")]
	public GameObject commandOrb = null;
	public bool commandOrbIsActive = false;
	public GameObject paintOrb = null;
	public bool paintOrbIsActive = false;
	public GameObject physicsOrb = null;
	public bool physicsOrbIsActive = false;
	public GameObject teleportOrb = null;
	public bool teleportOrbIsActive = false;
	[Header("Transform for Teleport")]
	public Transform playerRoor;
	[Header("Companion")]
	public Companion companion;

	public event Action<OrbType> OnOrbShot;
	public event Action<OrbType> OnChangeOrb;

	private MeshRenderer meshRenderer;
	private AudioSource audioSource;

	private OrbType currentOrb = OrbType.None;
	private CommandOrb commandOrbComponent;
	private PaintOrb paintOrbComponent;
	private PhysicsOrb physicsOrbComponent;
	private TeleportOrb teleportOrbComponent;

	private MeshRenderer viveTrackpadMeshRenderer = null;

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
		meshRenderer.enabled = false;

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

	public bool SetCurrentOrbTo(OrbType orbType)
	{
		if (currentOrb == orbType)
			return true;
		else if (orbType == OrbType.CommandOrb && commandOrbIsActive)
			meshRenderer.material.color = commandOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
		else if (orbType == OrbType.PaintOrb && paintOrbIsActive)
			meshRenderer.material.color = paintOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
		else if (orbType == OrbType.PhysicsOrb && physicsOrbIsActive)
			meshRenderer.material.color = physicsOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
		else if (orbType == OrbType.TeleportOrb && teleportOrbIsActive)
			meshRenderer.material.color = teleportOrb.GetComponent<MeshRenderer>().sharedMaterial.color;
		else
			return false;

		meshRenderer.enabled = true;
		currentOrb = orbType;

		if (OnChangeOrb != null)
			OnChangeOrb(currentOrb);

		return true;
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
		else
			return;

		GameObject orb = Instantiate(orbPrefab, transform.position, transform.rotation);
		Rigidbody orbsRigidbody = orb.GetComponent<Rigidbody>();

		orbsRigidbody.AddForce(transform.forward * orbSpeed);

		audioSource.Play();

		if (OnOrbShot != null)
			OnOrbShot(currentOrb);
	}

	public void SetOrbActive(OrbType orbType, Material material)
	{
		if (orbType == OrbType.CommandOrb)
			commandOrbIsActive = true;
		else if (orbType == OrbType.PaintOrb)
			paintOrbIsActive = true;
		else if (orbType == OrbType.PhysicsOrb)
			physicsOrbIsActive = true;
		else if (orbType == OrbType.TeleportOrb)
			teleportOrbIsActive = true;

		if (viveTrackpadMeshRenderer != null && material != null)
			viveTrackpadMeshRenderer.material = material;

		SetCurrentOrbTo(orbType);
	}

	public bool IsOrbActive(OrbType orbType)
	{
		if (orbType == OrbType.CommandOrb && commandOrbIsActive)
			return true;
		else if (orbType == OrbType.PaintOrb && paintOrbIsActive)
			return true;
		else if (orbType == OrbType.PhysicsOrb && physicsOrbIsActive)
			return true;
		else if (orbType == OrbType.TeleportOrb && teleportOrbIsActive)
			return true;
		else
			return false;
	}

	public void SetViveTrackpadMeshRenderer(MeshRenderer meshRenderer)
	{
		viveTrackpadMeshRenderer = meshRenderer;
	}
}
