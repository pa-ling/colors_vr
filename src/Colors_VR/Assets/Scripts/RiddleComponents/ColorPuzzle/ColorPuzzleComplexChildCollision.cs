using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleComplexChildCollision : MonoBehaviour {

    private ColorPuzzle parent;

    void Start()
    {
        parent = transform.parent.GetComponent<ColorPuzzle>();
    }

    void OnCollisionEnter(Collision collision)
    {
        parent.OnCollisionEnter(collision);
    }
}
