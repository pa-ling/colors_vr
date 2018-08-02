using UnityEngine;

/*
Class for adding the audio source of the game object to all children for easy and fast changing of all audio sources (for the animations of the teleport puzzle).
*/
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
