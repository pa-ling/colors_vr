using System;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
	[HideInInspector]
	public event Action OnHit;

	private bool hit;

	private Color defaultColor;

	private MeshRenderer meshRenderer;

	private void Start()
	{
		hit = false;

		meshRenderer = GetComponent<MeshRenderer>();

		defaultColor = meshRenderer.material.color;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PhysicsOrb>() == null)
			return;

		meshRenderer.material.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;

		Destroy(collision.gameObject);

		hit = true;

		if (OnHit != null)
			OnHit();
	}

	public bool IsHit()
	{
		return hit;
	}

	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}

	public void Reset()
	{
		hit = false;
		meshRenderer.material.color = defaultColor;
	}
}
