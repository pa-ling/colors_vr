using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;
    private AudioSource audio;

	private void Awake()
	{
		animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
	}

	public void OpenDoor()
	{
		animator.SetBool("Open", true);
        audio.Play();
	}

	public void CloseDoor()
	{
		animator.SetBool("Open", false);
	}
}
