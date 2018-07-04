using UnityEngine;

public class TakeOrb : MonoBehaviour {

    public OrbType orbType;
	public Material viveTrackpadMaterial;
	
	public AudioClip takeSound;

	[HideInInspector]
	public bool taken = false;

	private MeshRenderer meshRenderer;

	private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("OnTriggerEnter: " + other.gameObject.layer);
        if(other.gameObject.layer == 9 && !taken)
        {
            taken = true;

			OrbGun[] orbGuns = other.transform.root.GetComponentsInChildren<OrbGun>();

			foreach (OrbGun orbGun in orbGuns)
				orbGun.SetOrbActive(orbType, viveTrackpadMaterial);

			VRPlayerController vrPlayerController = other.transform.root.GetComponent<VRPlayerController>();

			if (vrPlayerController != null)
				vrPlayerController.Vibration(ViveController.BOTH, 1.0f);

			AudioSource.PlayClipAtPoint(takeSound, transform.position, 0.5f);

            meshRenderer.enabled = false;
        }
    }

	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}
}
