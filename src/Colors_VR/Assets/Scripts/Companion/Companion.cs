﻿using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
	private NavMeshAgent navMeshAgent;
	private Animator animator;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		float forward = Vector3.Dot(navMeshAgent.velocity, transform.forward) / navMeshAgent.velocity.magnitude;
		animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
		animator.SetFloat("Forward", (forward > 0) ? forward : 1.0f);
	}

	public void MoveTo(Vector3 position)
	{
		navMeshAgent.destination = position;
	}
}
