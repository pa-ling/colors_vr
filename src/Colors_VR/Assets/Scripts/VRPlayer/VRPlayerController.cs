﻿using System.Collections;
using UnityEngine;

public class VRPlayerController : MonoBehaviour
{
	[Header("Companion")]
	public Companion companion;

	[Header("OrbGuns")]
	public OrbGun leftOrbGun;
	public OrbGun rightOrbGun;

	[Header("Material")]
	public Material triggerMaterial;

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

		do
		{
			transform = steamVR_ControllerManager.left.transform.Find("Model/trackpad");
			yield return null;
		} while (transform == null);

		leftOrbGun.SetViveTrackpadMeshRenderer(transform.gameObject.GetComponent<MeshRenderer>());

		do
		{
			transform = steamVR_ControllerManager.left.transform.Find("Model/trigger");
			yield return null;
		} while (transform == null);

		transform.gameObject.GetComponent<MeshRenderer>().material = triggerMaterial;
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

		do
		{
			transform = steamVR_ControllerManager.right.transform.Find("Model/trackpad");
			yield return null;
		} while (transform == null);

		rightOrbGun.SetViveTrackpadMeshRenderer(transform.gameObject.GetComponent<MeshRenderer>());

		do
		{
			transform = steamVR_ControllerManager.right.transform.Find("Model/trigger");
			yield return null;
		} while (transform == null);

		transform.gameObject.GetComponent<MeshRenderer>().material = triggerMaterial;
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

	private IEnumerator BlinkingTrigger()
	{
		Color yellow = new Color(1.0f, 1.0f, 0.5f, 1.0f);
		Color diffrenceColor = new Color(0.1f, 0.1f, 0.05f, 0.0f);
		bool fromBlackToYellow = true;

		while (true)
		{
			Color newColor = triggerMaterial.color;

			if (fromBlackToYellow)
			{
				newColor += diffrenceColor;
			
				if (newColor == yellow)
					fromBlackToYellow = false;
			}
			else
			{
				newColor -= diffrenceColor;

				if (newColor == Color.black)
					fromBlackToYellow = true;
			}

			triggerMaterial.color = newColor;

			yield return new WaitForSeconds(Time.deltaTime * 3.5f);
		}
	}

	private IEnumerator BlinkingTrackpad()
	{
		Color yellow = new Color(1.0f, 1.0f, 0.5f, 1.0f);
		Color diffrenceColor = new Color(0.0f, 0.0f, 0.05f, 0.0f);
		bool fromWhiteToYellow = true;

		MeshRenderer leftMeshRenderer = leftOrbGun.GetViveTrackpadMeshRenderer();
		MeshRenderer rightMeshRenderer = rightOrbGun.GetViveTrackpadMeshRenderer();

		while (true)
		{
			Color newColor = leftMeshRenderer.material.color;

			if (fromWhiteToYellow)
			{
				newColor -= diffrenceColor;

				if (newColor == yellow)
					fromWhiteToYellow = false;
			}
			else
			{
				newColor += diffrenceColor;

				if (newColor == Color.white)
					fromWhiteToYellow = true;
			}

			leftMeshRenderer.material.color = newColor;
			rightMeshRenderer.material.color = newColor;

			yield return new WaitForSeconds(Time.deltaTime * 3.5f);
		}
	}

	public void StartBlinkingTrigger()
	{
		StartCoroutine(BlinkingTrigger());
	}

	public void StopBlinkTrigger()
	{
		StopCoroutine(BlinkingTrigger());
		triggerMaterial.color = Color.black;
	}

	public void StartBlinkingTrackpad()
	{
		StartCoroutine(BlinkingTrackpad());
	}

	public void StopBlinkTrackpad()
	{
		StopCoroutine(BlinkingTrackpad());

		leftOrbGun.GetViveTrackpadMeshRenderer().material.color = Color.white;
		rightOrbGun.GetViveTrackpadMeshRenderer().material.color = Color.white;
	}
}
