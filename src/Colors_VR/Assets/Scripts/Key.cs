using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public char character;
    private Keypad keypad;

	void Start ()
    {
        keypad = this.transform.parent.parent.gameObject.GetComponent<Keypad>();
	}

    protected virtual void OnCollisionEnter(Collision collision)
    {
        keypad.SendCharacter(character);
    }
}
