using UnityEngine;

public class ShootingPuzzle : MonoBehaviour
{
	public Door door;

	[Header("Targets")]
	public ShootingTarget targetOne;
	public ShootingTarget targetTwo;
	public ShootingTarget targetThree;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();

		targetOne.SetActive(false);
		targetTwo.SetActive(false);
		targetThree.SetActive(false);

		targetOne.OnHit += InvokeActivateSecondTarget;
		targetTwo.OnHit += InvokeActivateThirdTarget;
		targetThree.OnHit += InvokeOpenDoor;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9)
		{
			if (!targetOne.IsHit())
				InvokeActivateFirstTarget();
		}
	}

	private void InvokeActivateFirstTarget()
	{
		Invoke("ActivateFirstTarget", 1.0f);
	}

	private void InvokeActivateSecondTarget()
	{
		targetOne.SetActive(false);
		Invoke("ActivateSecondTarget", 1.0f);
	}

	private void InvokeActivateThirdTarget()
	{
		targetTwo.SetActive(false);
		Invoke("ActivateThirdTarget", 1.0f);
	}

	private void InvokeOpenDoor()
	{
		targetThree.SetActive(false);
		Invoke("OpenDoor", 1.0f);
	}

	private void ActivateFirstTarget()
	{
		targetOne.SetActive(true);
	}

	private void ActivateSecondTarget()
	{
		targetTwo.SetActive(true);

		animator.SetTrigger("ShootingTargetTwo");
	}

	private void ActivateThirdTarget()
	{
		targetThree.SetActive(true);

		animator.SetTrigger("ShootingTargetThree");
	}

	private void OpenDoor()
	{
		door.OpenDoor();
	}
}
