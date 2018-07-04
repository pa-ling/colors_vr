using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;
    private AudioSource audioSource;

    public bool openDoorThroughTriggerZone;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (openDoorThroughTriggerZone && !animator.GetBool("Open"))
        {
            if (other.gameObject.layer == 9)
				OpenDoor();
        }
    }

	public void OpenDoor()
	{
		animator.SetBool("Open", true);
		audioSource.Play();
	}

	public void CloseDoor()
	{
		animator.SetBool("Open", false);
	}
}
