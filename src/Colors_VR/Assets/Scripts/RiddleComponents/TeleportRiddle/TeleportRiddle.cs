using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TeleportRiddle : MonoBehaviour
{
	public Companion companion;
	public Lift lift;
	public Door door;

	private NavMeshSurface navMeshSurface;
	private NavMeshLink[] navMeshLinks;
	private Animator animator;

	private void Start()
	{
		navMeshSurface = GetComponent<NavMeshSurface>();
		navMeshLinks = GetComponents<NavMeshLink>();
		animator = GetComponent<Animator>();

		lift.OnCompanionEnterLift += LiftCompanionUp;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Companion"))
			BuildUp();

		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			navMeshSurface.BuildNavMesh();
			EnableCompanionsNavMeshAgent();
			door.OpenDoor();

			for (int i = 0; i < navMeshLinks.Length; ++i)
				navMeshLinks[i].area = 0;
		}
	}

	private void LiftCompanionUp()
	{
		StartCoroutine(LiftUp(1.0f));
	}

	private void BuildUp()
	{
		animator.SetTrigger("BuildUp");
	}

	private void EnableCompanionsNavMeshAgent()
	{
		companion.SetNavMeshAgentActive(true);
		companion.IgnoreCommand(false);
	}

	private IEnumerator LiftUp(float seconds)
	{
		companion.IgnoreCommand(true);

		yield return new WaitForSeconds(seconds);

		companion.SetNavMeshAgentActive(false);

		lift.LiftUp();
	}
}
