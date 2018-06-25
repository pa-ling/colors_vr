using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovableObject))]
public class MovableObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MovableObject myTarget = (MovableObject)target;

        EditorGUILayout.Vector3Field("StartPosition in world coordinates", myTarget.startPosition);
        if (GUILayout.Button("Set StartPosition"))
        {
            myTarget.startPosition = myTarget.transform.position;
        }
        if (GUILayout.Button("Set movable object back to StartPosition"))
        {
            myTarget.transform.position = myTarget.startPosition;
        }

        EditorGUILayout.Vector3Field("MaxPosition in world coordinates", myTarget.maxPosition);
        if (GUILayout.Button("Set max position"))
        {
            myTarget.maxPosition = myTarget.transform.position;
        }
        if (GUILayout.Button("Set movable object back to maxPosition"))
        {
            myTarget.transform.position = myTarget.maxPosition;
        }


    }
}
