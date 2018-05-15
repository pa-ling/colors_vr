using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float walkingSpeed = 5.0f;
	public float sensitivity = 3.0f;

	public Text infoText = null;

	private OrbGun orbGun = null;

	private void Start()
	{
		orbGun = GetComponentInChildren<OrbGun>();

		infoText.text = orbGun.GetCurrentOrb().ToString();
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
		Orb orb = orbGun.GetCurrentOrb();

		if (orb == Orb.TeleportOrb)
			orb = Orb.FluidOrb;
		else
			++orb;

		orbGun.SetCurrentOrbTo(orb);
		infoText.text = orb.ToString();
	}
}
