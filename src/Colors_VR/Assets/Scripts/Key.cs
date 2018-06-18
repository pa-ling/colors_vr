using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public char character;
    private Keypad keypad;
    private MeshRenderer meshRenderer;

	void Start ()
    {
        keypad = this.transform.parent.parent.gameObject.GetComponent<Keypad>();
        meshRenderer = GetComponent<MeshRenderer>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PhysicsOrb>() == null)
            return;
            
        keypad.SendCharacter(character);
        meshRenderer.material.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;
    }
}
