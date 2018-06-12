using System;
using UnityEngine;

public class Lift : MonoBehaviour
{
	[HideInInspector]
	public bool companionSitsOn = false;
	public event Action OnCompanionEnterLift;
	public event Action OnCompanionLeaveLift;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void CompanionEntersLift()
	{
		companionSitsOn = true;
		if (OnCompanionEnterLift != null)
			OnCompanionEnterLift();
	}

	public void CompanionLeftLift()
	{
		companionSitsOn = false;
		if (OnCompanionLeaveLift != null)
			OnCompanionLeaveLift();
	}

	public bool IsLifting()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("LiftUp"))
			return true;

		return false;
	}

	public void LiftUp()
	{
		animator.SetTrigger("LiftUp");
	}
}
