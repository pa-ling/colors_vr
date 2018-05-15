using UnityEngine;

public class FluidOrb : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("FluidOrb hit");
	}
}
