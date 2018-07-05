using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
	private NavMeshAgent navMeshAgent;
	private Animator animator;

    private AudioSource movementAudioSource;
    private AudioSource speakingAudioSource;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		movementAudioSource = GetComponents<AudioSource>()[0];
		speakingAudioSource = GetComponents<AudioSource>()[1];
	}

	private void Update()
	{
		float forward = Vector3.Dot(navMeshAgent.velocity, transform.forward) / navMeshAgent.velocity.magnitude;
		animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
		animator.SetFloat("Forward", (forward > 0) ? forward : 1.0f);

        if(navMeshAgent.velocity != Vector3.zero)
        {
            if (!movementAudioSource.isPlaying)
            {
				movementAudioSource.Play();
            }
        }
        else
        {
			movementAudioSource.Pause();
        }

        transform.LookAt(Camera.main.transform);
	}

	public void MoveTo(Vector3 position)
	{
		navMeshAgent.destination = position;
	}

    public void StartSpeaking(AudioClip audioClip)
    {
		speakingAudioSource.Stop();
		speakingAudioSource.clip = audioClip;
		speakingAudioSource.Play();
    }
}
