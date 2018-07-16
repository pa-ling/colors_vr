using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
	[HideInInspector]
	public Transform[] autoFollowTransforms;

	public AudioClip[] idleSounds;

	private bool idle;
	private int hitByOrb;

	private bool autoFollow;
	private int lastAutoFollowIndex;
	private Vector3 lastAutoFollowPosition;

	private NavMeshAgent navMeshAgent;
	private Animator animator;

    private AudioSource movementAudioSource;
    private AudioSource speakingAudioSource;

	private MeshRenderer meshRenderer;

	private void Start()
	{
		idle = false;
		autoFollow = false;
		hitByOrb = 0;

		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		movementAudioSource = GetComponents<AudioSource>()[0];
		speakingAudioSource = GetComponents<AudioSource>()[1];
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	private void Update()
	{
		transform.LookAt(Camera.main.transform);

		if (autoFollowTransforms != null)
		{
			if (autoFollowTransforms[lastAutoFollowIndex].position != lastAutoFollowPosition && autoFollow)
			{
				int nearestAutoFollowPositionIndex = 0;
				for (int i = 1; i < autoFollowTransforms.Length; ++i)
				{
					float bestCurrentDistance = Vector3.Distance(transform.position, autoFollowTransforms[nearestAutoFollowPositionIndex].position);
					float otherDistance = Vector3.Distance(transform.position, autoFollowTransforms[i].position);

					if (otherDistance < bestCurrentDistance)
						nearestAutoFollowPositionIndex = i;
				}

				MoveTo(autoFollowTransforms[nearestAutoFollowPositionIndex].position);
				lastAutoFollowIndex = nearestAutoFollowPositionIndex;
				lastAutoFollowPosition = autoFollowTransforms[nearestAutoFollowPositionIndex].position;
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

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Orb"))
		{
			if (autoFollow)
			{
				++hitByOrb;

				if (hitByOrb == 5)
					StartSpeaking(idleSounds[0]);
				else if (hitByOrb == 20)
				{
					StartSpeaking(idleSounds[1]);
					hitByOrb = 0;
				}
			}
		}
	}

	public void SetIdle(bool value)
	{
		idle = value;
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
