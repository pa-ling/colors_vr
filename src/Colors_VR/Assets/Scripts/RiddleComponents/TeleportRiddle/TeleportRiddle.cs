using UnityEngine;

public class TeleportRiddle : MonoBehaviour
{
	public Companion companion;
	public Door door;
	public PressButton pressButton;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();

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
}
