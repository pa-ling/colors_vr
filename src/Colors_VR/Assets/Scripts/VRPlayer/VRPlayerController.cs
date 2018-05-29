using System.Collections;
using UnityEngine;

public class VRPlayerController : MonoBehaviour
{
	public OrbGun orbGun;
	public Material touchPadMaterial;

	private SteamVR_Controller.Device leftController;
	private SteamVR_Controller.Device rightController;

	private void Start()
	{
		StartCoroutine(SetupControllers());
	}

	private void Update()
	{
		if (rightController != null)
		{
			if (rightController.GetHairTriggerDown())
				orbGun.Fire();

			if (rightController.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				ChangeOrbType(rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad));
		}
	}

	private IEnumerator SetupControllers()
	{
		do
		{
			leftController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
			yield return null;
		} while (leftController == null);

		do
		{
			rightController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));
			yield return null;
		} while (rightController == null);

		SteamVR_ControllerManager steamVR_ControllerManager = GetComponent<SteamVR_ControllerManager>();
		Transform touchPad = null;

		do
		{
			touchPad = steamVR_ControllerManager.right.transform.Find("Model/trackpad");
			yield return null;
		} while (touchPad == null);

		touchPad.gameObject.GetComponent<MeshRenderer>().material = touchPadMaterial;
	}

	private void ChangeOrbType(Vector2 position)
	{
		Vector2 leftBottomToRightTop = new Vector2(2.0f, 2.0f);
		Vector2 rightBottomToLeftTop = new Vector2(-2.0f, 2.0f);

		bool leftTop = (-leftBottomToRightTop.x * position.y + leftBottomToRightTop.y * position.x) < 0;
		bool rightTop = (-rightBottomToLeftTop.x * position.y + rightBottomToLeftTop.y * position.x) > 0;

		if (leftTop && rightTop)
		{
			orbGun.SetCurrentOrbTo(OrbType.TeleportOrb);
			Vibration();
		}
		else if (!leftTop && rightTop)
		{
			orbGun.SetCurrentOrbTo(OrbType.PaintOrb);
			Vibration();
		}
		else if (leftTop && !rightTop)
		{
			orbGun.SetCurrentOrbTo(OrbType.CommandOrb);
			Vibration();
		}
		else if (!leftTop && !rightTop)
		{
			orbGun.SetCurrentOrbTo(OrbType.PhysicsOrb);
			Vibration();
		}
	}

	private IEnumerator VibrateControllers(float durationInSeconds = 0.1f)
	{
		for (float seconds = 0; seconds < durationInSeconds; seconds += 0.01f)
		{
			if (leftController != null)
				leftController.TriggerHapticPulse();

			if (rightController != null)
				rightController.TriggerHapticPulse();

			yield return new WaitForSeconds(0.01f);
		}
	}

	public void Vibration(float durationInSeconds = 0.1f)
	{
		StopCoroutine(VibrateControllers());
		StartCoroutine(VibrateControllers(durationInSeconds));
	}
}
