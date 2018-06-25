using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeOrb : MonoBehaviour {

    [HideInInspector]
    public OrbType orbType;
    private bool alreadyTaken = false;
    private MeshRenderer meshRenderer;

    public AudioClip takeSound;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        orbType = (OrbType)Enum.Parse(typeof(OrbType), name);
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9 && !alreadyTaken)         //"Player"
        {
            other.gameObject.GetComponent<PlayerController>().addOrb(orbType);
            alreadyTaken = true;
            AudioSource otherAudioSource;
            if (!(otherAudioSource = other.gameObject.GetComponent<AudioSource>()))
            {
                other.gameObject.AddComponent<AudioSource>();
                otherAudioSource = other.gameObject.GetComponent<AudioSource>();
            }
            otherAudioSource.volume = 0.5f;
            otherAudioSource.clip = takeSound;
            otherAudioSource.Play();
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(takeSound.length);
            Destroy(otherAudioSource);
            Destroy(gameObject);
        }
    }

}
