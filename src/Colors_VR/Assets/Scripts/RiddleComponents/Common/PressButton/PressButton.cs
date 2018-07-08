using System;
using UnityEngine;

public class PressButton : MonoBehaviour
{
	[HideInInspector]
	public event Action OnPressButtonHit;

	private Color defaultColor;

	private MeshRenderer meshRenderer;

	private void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();

		defaultColor = meshRenderer.material.color;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PhysicsOrb>() == null)
			return;

		meshRenderer.material.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;

		Destroy(collision.gameObject);

		if (OnPressButtonHit != null)
			OnPressButtonHit();
	}

	public void ResetColor()
	{
		meshRenderer.material.color = defaultColor;
	}
}
