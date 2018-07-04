using System;
using UnityEngine;

public class Lift : MonoBehaviour
{
	[HideInInspector]
	public bool companionSitsOn = false;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void LiftUp()
	{
		animator.SetTrigger("LiftUp");
	}
}
