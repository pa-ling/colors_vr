using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAudioToChildren : MonoBehaviour {

    private AudioSource audioSource;

	public void attachAudio()
    {
        audioSource = GetComponent<AudioSource>();
        UnityEditorInternal.ComponentUtility.CopyComponent(audioSource);

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject.GetComponent<AudioSource>());
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(child.gameObject);
        }

    }

}
