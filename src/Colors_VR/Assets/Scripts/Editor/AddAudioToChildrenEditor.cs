using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
Editor-Class for adding the audio source of the game object to all children for easy and fast changing of all audio sources (for the animations of the teleport puzzle).
Shows Button and works in Editor.
*/
[CustomEditor(typeof(AddAudioToChildren))]
public class AddAudioToChildrenEditor : Editor {

    public override void OnInspectorGUI()
    {
        AddAudioToChildren myTarget = (AddAudioToChildren)target;



        if(GUILayout.Button("Attach AudioSource to children"))
        {
            myTarget.attachAudio();
        }
    }
}
