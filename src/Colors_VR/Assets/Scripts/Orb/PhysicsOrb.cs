using UnityEngine;

public class PhysicsOrb : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("PhysicsOrb hit");
	}
}
