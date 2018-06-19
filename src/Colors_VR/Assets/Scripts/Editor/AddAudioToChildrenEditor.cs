using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
