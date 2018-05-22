using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Orb : MonoBehaviour
{
	public float splatMinSize = 0.5f;
	public float splatMaxSize = 1.5f;

	private MeshRenderer meshRenderer;
	private SplatParticleSystem splatParticleSystem = null;

	protected void Start()
	{
		splatParticleSystem = GameObject.Find("SplatParticleSystem").GetComponent<SplatParticleSystem>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		SplatParticle splatParticle = new SplatParticle();
		splatParticle.position = collision.contacts[0].point;
		splatParticle.position += collision.contacts[0].normal * 0.001f;
		splatParticle.rotation = Quaternion.LookRotation(collision.contacts[0].normal).eulerAngles;
		splatParticle.rotation.z = Random.Range(0.0f, 360.0f);
		splatParticle.size = Random.Range(splatMinSize, splatMaxSize);
		splatParticle.color = meshRenderer.material.color;

		splatParticleSystem.AddSplatParticle(splatParticle);
	}
}
