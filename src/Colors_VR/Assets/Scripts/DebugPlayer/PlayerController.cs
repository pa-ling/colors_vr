using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Companion companion;

	[Header("Movement")]
	public float walkingSpeed = 5.0f;
	public float sensitivity = 3.0f;

	private OrbGun orbGun = null;

	private void Start()
	{
		companion.autoFollowTransforms = GetComponentInChildren<AutoFollowPosition>().GetAutoFollowPositions();

		orbGun = GetComponentInChildren<OrbGun>();

        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		float strafe = Input.GetAxis("Horizontal") * Time.deltaTime * walkingSpeed;
		float forwardBackward = Input.GetAxis("Vertical") * Time.deltaTime * walkingSpeed;
		float mouseY = Input.GetAxis("Mouse Y") * -sensitivity;
		float mouseX = Input.GetAxis("Mouse X") * sensitivity;

		transform.Translate(strafe, 0.0f, forwardBackward);
		transform.Rotate(0.0f, mouseX, 0.0f);
		transform.Find("[Camera]").transform.Rotate(mouseY, 0.0f, 0.0f);

        if (Input.GetKeyDown(KeyCode.Mouse0))
			orbGun.Fire();

		if (Input.GetKeyDown(KeyCode.Mouse1))
			ChangeOrbType();
	}

	private void ChangeOrbType()
	{
		OrbType orb = orbGun.GetCurrentOrb();

		if (orb == OrbType.None)
			return;

		do
		{
			if (orb == OrbType.TeleportOrb)
				orb = OrbType.CommandOrb;
			else
				++orb;
		} while(!orbGun.SetCurrentOrbTo(orb));
	}
}
