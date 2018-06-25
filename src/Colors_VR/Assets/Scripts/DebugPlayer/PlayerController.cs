﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float walkingSpeed = 5.0f;
	public float sensitivity = 3.0f;

	private OrbGun orbGun = null;
    private List<OrbType> orbs;

	private void Start()
	{
        orbGun = GetComponentInChildren<OrbGun>();
        orbs = new List<OrbType>();

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

        if(orbs.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                orbGun.Fire();

            if (Input.GetKeyDown(KeyCode.Mouse1))
                ChangeOrbType();
        }

	}

	private void ChangeOrbType()
	{
		OrbType orb = orbGun.GetCurrentOrb();
        if(orb == OrbType.None)
        {
            orb = orbs[0];
        }

        int index = orbs.IndexOf(orb);
        index = ((index + 1) >= orbs.Count) ? 0 : index+1;
        orb = orbs[index];

		orbGun.SetCurrentOrbTo(orb);
	}

    public void addOrb(OrbType orb)
    {
        orbs.Add(orb);
        ChangeOrbType();
    }
}
