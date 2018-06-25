using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
	private NavMeshAgent navMeshAgent;
	private Animator animator;
	private bool ignoreCommands = false;

    private AudioSource movementSound;
    private AudioSource speakingSound;

    public AudioClip[] speakingClips;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
        movementSound = GetComponents<AudioSource>()[0];
        speakingSound = GetComponents<AudioSource>()[1];
        StartCoroutine("StartSpeaking");
	}

	private void Update()
	{
		float forward = Vector3.Dot(navMeshAgent.velocity, transform.forward) / navMeshAgent.velocity.magnitude;
		animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
		animator.SetFloat("Forward", (forward > 0) ? forward : 1.0f);

        if(navMeshAgent.velocity != Vector3.zero)
        {
            if (!movementSound.isPlaying)
            {
                movementSound.Play();
            }
        }
        else
        {
            movementSound.Pause();
        }


	}

	private void OnTriggerEnter(Collider other)
	{
		Lift lift = other.gameObject.GetComponent<Lift>();
		if (lift != null)
		{
			if (transform.parent == null)
				transform.SetParent(other.gameObject.transform);
			lift.CompanionEntersLift();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Lift lift = other.gameObject.GetComponent<Lift>();
		if (lift != null)
		{
			transform.SetParent(null);
			lift.CompanionLeftLift();
		}
	}

	public void MoveTo(Vector3 position)
	{
		if (!ignoreCommands)
			navMeshAgent.destination = position;
	}

	public void IgnoreCommand(bool state)
	{
		ignoreCommands = state;
	}

	public void SetNavMeshAgentActive(bool state)
	{
		navMeshAgent.enabled = state;
	}

    private IEnumerator StartSpeaking()
    {
        yield return new WaitForSeconds(5);
        speakingSound.clip = speakingClips[0];
        speakingSound.Play();
        yield return null;
    }
}
