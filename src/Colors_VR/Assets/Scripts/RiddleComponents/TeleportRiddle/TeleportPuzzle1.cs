using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPuzzle1 : MonoBehaviour {

    public Door door;
    public PressButton pressButton;

    void Start () {
        pressButton.OnPressButtonHit += door.OpenDoor;
    }

}
