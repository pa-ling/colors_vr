using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void OpenDoor()
	{
		animator.SetBool("Open", true);
	}

	public void CloseDoor()
	{
		animator.SetBool("Open", false);
	}
}
