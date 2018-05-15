using UnityEngine;

public class PaintOrb : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("PaintOrb hit");
	}
}
