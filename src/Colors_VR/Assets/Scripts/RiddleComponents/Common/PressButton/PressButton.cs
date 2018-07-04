using System;
using UnityEngine;

public class PressButton : MonoBehaviour
{
	[HideInInspector]
	public event Action OnPressButtonHit;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PhysicsOrb>() == null)
			return;

		if (OnPressButtonHit != null)
			OnPressButtonHit();
	}
}
