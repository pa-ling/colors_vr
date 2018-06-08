using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleComplexChildCollision : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<ColorPuzzle>().OnCollisionEnter(collision);
    }
}
