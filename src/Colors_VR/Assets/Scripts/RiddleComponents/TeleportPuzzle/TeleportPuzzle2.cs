using UnityEngine;
using UnityEngine.AI;

public class TeleportPuzzle2 : MonoBehaviour
{
	public Companion companion;
	public Door door;
	public PressButton pressButton;

	private Animator animator;
	private NavMeshSurface navMeshSurface;

	private void Start()
	{
		animator = GetComponent<Animator>();
		navMeshSurface = GetComponentInParent<NavMeshSurface>();

		pressButton.OnPressButtonHit += BuildUp;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			door.OpenDoor();
	}

	private void BuildUp()
	{
		animator.SetTrigger("BuildUp");
	}

	public void BakeNewNavMesh()
	{
		navMeshSurface.BuildNavMesh();
	}
}
