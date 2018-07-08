using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
	[HideInInspector]
	public Transform autoFollowTransform;

	private bool autoFollow;
	private Vector3 lastAutoFollowPosition;

	private NavMeshAgent navMeshAgent;
	private Animator animator;

    private AudioSource movementAudioSource;
    private AudioSource speakingAudioSource;

	private MeshRenderer meshRenderer;

	private void Start()
	{
		autoFollow = false;

		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		movementAudioSource = GetComponents<AudioSource>()[0];
		speakingAudioSource = GetComponents<AudioSource>()[1];
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	private void Update()
	{
		Debug.Log(IsMoving());
		transform.LookAt(Camera.main.transform);

		if (autoFollowTransform != null)
		{
			if (autoFollowTransform.position != lastAutoFollowPosition && autoFollow)
			{
				MoveTo(autoFollowTransform.position);
				lastAutoFollowPosition = autoFollowTransform.position;
			}
		}

		if (navMeshAgent.velocity != Vector3.zero)
        {
            if (!movementAudioSource.isPlaying)
				movementAudioSource.Play();
        }
        else
			movementAudioSource.Pause();
	}

	public void SetAutoFollow(bool value)
	{
		autoFollow = value;
	}

	public void MoveTo(Vector3 position)
	{
		navMeshAgent.destination = position;
	}

	public bool IsMoving()
	{
		return navMeshAgent.velocity.magnitude != 0.0f;
	}

    public void StartSpeaking(AudioClip audioClip)
    {
		speakingAudioSource.Stop();
		speakingAudioSource.clip = audioClip;
		speakingAudioSource.Play();
    }

	public bool IsVisibleByAnyCamera()
	{
		return meshRenderer.isVisible;
	}
}
