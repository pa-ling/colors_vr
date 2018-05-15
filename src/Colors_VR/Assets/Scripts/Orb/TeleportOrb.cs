using UnityEngine;

public class TeleportOrb : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("TeleportOrb hit");
	}
}
