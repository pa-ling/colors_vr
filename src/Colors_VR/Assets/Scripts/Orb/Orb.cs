using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Orb : MonoBehaviour
{
	public float splatMinSize = 0.5f;
	public float splatMaxSize = 1.5f;
	public float speed = 500.0f;
    public int colorChannel = 3;

    public AudioClip splashSound;

	[HideInInspector]
	public LayerMask dontLeaveSplatsOn;
	[HideInInspector]
	public ParticleSystem dropletParticleSystem = null;

	protected MeshRenderer meshRenderer;
	private SplatParticleSystem splatParticleSystem = null;

    protected void Start()
	{
		splatParticleSystem = GameObject.Find("SplatParticleSystem").GetComponent<SplatParticleSystem>();
		dropletParticleSystem = GameObject.Find("DropletParticleSystem").GetComponent<ParticleSystem>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
        if ((dontLeaveSplatsOn & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
			return;

        if (collision.gameObject.GetComponent<Paintable>() != null)
            SplatOnVertices(collision);
		else
            Splat(collision);

		AudioSource.PlayClipAtPoint(splashSound, transform.position, 35);
	}

	private void Splat(Collision collision)
	{
		SplatParticle splatParticle = new SplatParticle();
		splatParticle.position = collision.contacts[0].point;
		splatParticle.position += collision.contacts[0].normal * 0.001f;
		splatParticle.rotation = Quaternion.LookRotation(collision.contacts[0].normal).eulerAngles;
		splatParticle.rotation.z = Random.Range(0.0f, 360.0f);
		splatParticle.size = Random.Range(splatMinSize, splatMaxSize);
		splatParticle.color = meshRenderer.material.color;

		splatParticleSystem.AddSplatParticle(splatParticle);

		dropletParticleSystem.gameObject.transform.position = collision.contacts[0].point;
		dropletParticleSystem.gameObject.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
		ParticleSystem.ShapeModule shapeModule = dropletParticleSystem.shape;
		shapeModule.radius = 0.25f * splatParticle.size;
		ParticleSystem.MainModule mainModule = dropletParticleSystem.main;
		mainModule.startColor = meshRenderer.material.color;
		dropletParticleSystem.Emit(Random.Range(3, 6));
	}

    private void SplatOnVertices (Collision collision)
    {
        collision.collider.GetComponent<Paintable>().ApplyPaint(collision.contacts[0].point, 0.05f, 0.3f, colorChannel);
    }
}
