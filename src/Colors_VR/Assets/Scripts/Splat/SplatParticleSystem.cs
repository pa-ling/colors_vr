using UnityEngine;

public class SplatParticleSystem : MonoBehaviour
{
	public int maxSplats = 1000;

	private int splatIndex = 0;
	private ParticleSystem splatParticleSystem;
	private ParticleSystem.Particle[] particles;

	private void Start()
	{
		splatParticleSystem = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[maxSplats];
	}

	public void AddSplatParticle(SplatParticle splatParticle)
	{
		if (splatIndex >= maxSplats)
			splatIndex = 0;

		particles[splatIndex].position = splatParticle.position;
		particles[splatIndex].rotation3D = splatParticle.rotation;
		particles[splatIndex].startSize = splatParticle.size;
		particles[splatIndex].startColor = splatParticle.color;

		++splatIndex;

		splatParticleSystem.SetParticles(particles, maxSplats);
	}
}
