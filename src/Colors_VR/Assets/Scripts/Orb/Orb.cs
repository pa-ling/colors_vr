using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Orb : MonoBehaviour
{
	public float splatMinSize = 1.5f;
	public float splatMaxSize = 2f;
	public float speed = 500.0f;

    public AudioClip splashSound;
    public GameObject splatDecal;
    public Material splatMaterial;

	[HideInInspector]
	public LayerMask dontLeaveSplatsOn;
	[HideInInspector]
	public ParticleSystem dropletParticleSystem = null;
    private ZFighting ZFighting;

	protected MeshRenderer meshRenderer;
    private SplatParticleSystem splatParticleSystem = null;

    protected virtual void Start()
	{
        splatParticleSystem = GameObject.Find("SplatParticleSystem").GetComponent<SplatParticleSystem>();
        dropletParticleSystem = GameObject.Find("DropletParticleSystem").GetComponent<ParticleSystem>();
		meshRenderer = GetComponent<MeshRenderer>();

        ZFighting = GameObject.Find("[ParticleSystems]").GetComponent<ZFighting>();
	}

	protected void Splat(Collision collision)
	{
        //SplatParticle splatParticle = new SplatParticle();
        //splatParticle.position = collision.contacts[0].point;
        //splatParticle.position += collision.contacts[0].normal * 0.001f;
        //splatParticle.rotation = Quaternion.LookRotation(collision.contacts[0].normal).eulerAngles;
        //splatParticle.rotation.z = UnityEngine.Random.Range(0.0f, 360.0f);
        //splatParticle.size = UnityEngine.Random.Range(splatMinSize, splatMaxSize);
        //splatParticle.color = meshRenderer.material.color;

        //splatParticleSystem.AddSplatParticle(splatParticle);


        
        //splat needs an empty gameobject as parent for scaling
        GameObject parentSplat = Instantiate(splatDecal, collision.contacts[0].point, Quaternion.identity);
        parentSplat.transform.parent = collision.gameObject.transform;
        GameObject splat = parentSplat.transform.GetChild(0).gameObject;

        float newX = 0;

        _Decal.Decal decal = splat.GetComponent<_Decal.Decal>();
        if (decal)
        {
            decal.transform.forward = -collision.contacts[0].normal;
            decal.material = splatMaterial;
            decal.maxAngle = 1;

            //randomize x, scale accordingly
            newX = UnityEngine.Random.Range(splatMinSize, splatMaxSize);
            Vector3 scale = new Vector3(newX, newX / (decal.sprite.rect.width / decal.sprite.rect.height), splat.transform.localScale.z);
            splat.transform.localScale = scale;

            //randomize z-angle
            Vector3 euler = splat.transform.eulerAngles;
            euler.z = UnityEngine.Random.Range(0f, 360f);
            splat.transform.eulerAngles = euler;



            _Decal.DecalBuilder.BuildAndSetDirty(decal);
        }

        parentSplat.transform.position += collision.contacts[0].normal * ZFighting.ZFightingDistanceStart;
        ZFighting.ZFightingDistanceStart += ZFighting.ZFightingDistanceStep;


        dropletParticleSystem.gameObject.transform.position = collision.contacts[0].point;
        dropletParticleSystem.gameObject.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        ParticleSystem.ShapeModule shapeModule = dropletParticleSystem.shape;
        shapeModule.radius = 0.25f * newX;
        //shapeModule.radius = 0.25f * splatParticle.size;
        ParticleSystem.MainModule mainModule = dropletParticleSystem.main;
        mainModule.startColor = meshRenderer.material.color;
        dropletParticleSystem.Emit(UnityEngine.Random.Range(3, 6));
    }

}
