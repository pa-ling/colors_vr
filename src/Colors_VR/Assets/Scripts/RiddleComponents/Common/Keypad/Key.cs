using UnityEngine;

public class Key : MonoBehaviour
{

    public char character;
    private Keypad keypad;
    private MeshRenderer meshRenderer;

	private Color startingColor;

	private void Start()
    {
        keypad = this.transform.parent.parent.gameObject.GetComponent<Keypad>();
		keypad.OnResetKeyColor += ResetColor;

        meshRenderer = GetComponent<MeshRenderer>();
		startingColor = meshRenderer.material.color;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PhysicsOrb>() == null)
            return;

		if (keypad.isSolved)
			return;

		meshRenderer.material.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;
		keypad.SendCharacter(character);
    }

	private void ResetColor()
	{
		meshRenderer.material.color = startingColor;
	}
}
