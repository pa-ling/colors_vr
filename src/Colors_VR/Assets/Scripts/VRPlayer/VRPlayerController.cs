using System.Collections;
using UnityEngine;

public class VRPlayerController : MonoBehaviour
{
	[Header("Companion")]
	public Companion companion;

	[Header("OrbGunPrefab")]
	public GameObject orbGunPrefab;

	[Header("HintsForLeftController")]
	public GameObject triggerHintLeft;
	public GameObject touchpadHintLeft;

	[Header("HintsForRightController")]
	public GameObject triggerHintRight;
	public GameObject touchpadHintRight;

	private OrbGun leftOrbGun;
	private OrbGun rightOrbGun;

	private SteamVR_Controller.Device leftController;
	private SteamVR_Controller.Device rightController;

	private void Start()
	{
		companion.autoFollowTransforms = GetComponentInChildren<AutoFollowPosition>().GetAutoFollowPositions();

		StartCoroutine(SetupLeftController());
		StartCoroutine(SetupRightController());
	}

	private void Update()
	{
		if (rightController != null)
		{
			if (rightController.GetHairTriggerDown())
				rightOrbGun.Fire();

			if (rightController.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				ChangeOrbType(rightOrbGun, ViveController.RIGHT, rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad));
		}
		else
		{
			StopCoroutine(SetupRightController());
			StartCoroutine(SetupRightController());
		}

		if (leftController != null)
		{
			if (leftController.GetHairTriggerDown())
				leftOrbGun.Fire();

			if (leftController.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				ChangeOrbType(leftOrbGun, ViveController.LEFT, leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad));
		}
		else
		{
			StopCoroutine(SetupLeftController());
			StartCoroutine(SetupLeftController());
		}
	}

	private IEnumerator SetupLeftController()
	{
		do
		{
			leftController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
			yield return null;
		} while (leftController == null);

		SteamVR_ControllerManager steamVR_ControllerManager = GetComponent<SteamVR_ControllerManager>();
		Transform transform = null;

		Instantiate(orbGunPrefab, steamVR_ControllerManager.left.transform);

		do
		{
			leftOrbGun = steamVR_ControllerManager.left.GetComponentInChildren<OrbGun>();
			yield return null;
		} while (leftOrbGun == null);

		leftOrbGun.SetPlayerRootTransform(gameObject.transform);
		leftOrbGun.SetCompanion(companion);

		do
		{
			transform = steamVR_ControllerManager.left.transform.Find("Model/trackpad");
			yield return null;
		} while (transform == null);

		leftOrbGun.SetViveTrackpadMeshRenderer(transform.gameObject.GetComponent<MeshRenderer>());
	}

	private IEnumerator SetupRightController()
	{
		do
		{
			rightController = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));
			yield return null;
		} while (rightController == null);

		SteamVR_ControllerManager steamVR_ControllerManager = GetComponent<SteamVR_ControllerManager>();
		Transform transform = null;

		Instantiate(orbGunPrefab, steamVR_ControllerManager.right.transform);

		do
		{
			rightOrbGun = steamVR_ControllerManager.right.GetComponentInChildren<OrbGun>();
			yield return null;
		} while (rightOrbGun == null);

		rightOrbGun.SetPlayerRootTransform(gameObject.transform);
		rightOrbGun.SetCompanion(companion);

		do
		{
			transform = steamVR_ControllerManager.right.transform.Find("Model/trackpad");
			yield return null;
		} while (transform == null);

		rightOrbGun.SetViveTrackpadMeshRenderer(transform.gameObject.GetComponent<MeshRenderer>());
	}

	private void ChangeOrbType(OrbGun orbGun, ViveController viveController, Vector2 position)
	{
		Vector2 bottomToTop = Vector2.up;
		Vector2 leftBottomToRightTop = new Vector2(2.0f, 2.0f);
		Vector2 rightBottomToLeftTop = new Vector2(-2.0f, 2.0f);

		bool left = (-bottomToTop.x * position.y + bottomToTop.y * position.x) < 0;
		bool leftTop = (-leftBottomToRightTop.x * position.y + leftBottomToRightTop.y * position.x) < 0;
		bool rightTop = (-rightBottomToLeftTop.x * position.y + rightBottomToLeftTop.y * position.x) > 0;

		int numberOfActiveOrbs = 0;

		for (OrbType orbType = OrbType.CommandOrb; orbType <= OrbType.TeleportOrb; ++orbType)
		{
			if (orbGun.IsOrbActive(orbType))
				++numberOfActiveOrbs;
		}

		OrbType newOrb = OrbType.None;

		if (numberOfActiveOrbs == 2)
			newOrb = GetOrbTypeForTwoActiveOrbs(left);
		else if (numberOfActiveOrbs == 3)
			newOrb = GetOrbTypeForFourActiveOrbs(left, leftTop, rightTop);
		else if (numberOfActiveOrbs == 4)
			newOrb = GetOrbTypeForFourActiveOrbs(leftTop, rightTop);

		if (newOrb != OrbType.None)
		{
			if (orbGun.SetCurrentOrbTo(newOrb))
				Vibration(viveController);
		}
	}

	private OrbType GetOrbTypeForTwoActiveOrbs(bool left)
	{
		if (left)
			return OrbType.TeleportOrb;
		else if (!left)
			return OrbType.PaintOrb;
		else
			return OrbType.None;
	}

	private OrbType GetOrbTypeForFourActiveOrbs(bool left, bool leftTop, bool rightTop)
	{
		if (!leftTop && !rightTop)
			return OrbType.PhysicsOrb;
		else if (left && leftTop)
			return OrbType.TeleportOrb;
		else if (!left && !leftTop)
			return OrbType.PaintOrb;
		else
			return OrbType.None;
	}

	private OrbType GetOrbTypeForFourActiveOrbs(bool leftTop, bool rightTop)
	{
		if (leftTop && rightTop)
			return OrbType.TeleportOrb;
		else if (!leftTop && rightTop)
			return OrbType.PaintOrb;
		else if (leftTop && !rightTop)
			return OrbType.CommandOrb;
		else if (!leftTop && !rightTop)
			return OrbType.PhysicsOrb;
		else
			return OrbType.None;
	}

	private IEnumerator VibrateControllers(ViveController viveController, float durationInSeconds = 0.1f)
	{
		for (float seconds = 0; seconds < durationInSeconds; seconds += 0.01f)
		{
			if (leftController != null && viveController != ViveController.RIGHT)
				leftController.TriggerHapticPulse();

			if (rightController != null && viveController != ViveController.LEFT)
				rightController.TriggerHapticPulse();

			yield return new WaitForSeconds(0.01f);
		}
	}

	public void Vibration(ViveController viveController, float durationInSeconds = 0.1f)
	{
		StopCoroutine(VibrateControllers(viveController, durationInSeconds));
		StartCoroutine(VibrateControllers(viveController, durationInSeconds));
	}

	public void SetTriggerHintActive(bool value)
	{
		triggerHintLeft.SetActive(value);
		triggerHintRight.SetActive(value);
	}

	public void SetTouchpadHintActive(bool value)
	{
		touchpadHintLeft.SetActive(value);
		touchpadHintRight.SetActive(value);
	}
}
