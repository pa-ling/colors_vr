using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OpenDoor();
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			CloseDoor();
		}
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
