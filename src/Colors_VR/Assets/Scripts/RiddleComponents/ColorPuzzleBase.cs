using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
    public GameObject door;

    public void checkSolution()
    {
        Renderer[] rend;

        foreach (Transform child in transform)
        {
            if(child.gameObject.GetComponent<Renderer>() == null)
            {
                rend = child.gameObject.GetComponentsInChildren<Renderer>();
            }
            else
            {
                rend = new Renderer[] { child.gameObject.GetComponent<Renderer>() };
            }

            foreach(Renderer render in rend)
            {
                if(render.material.color == Color.white)
                {
                    correct = false;
                    goto falseSolution;
                }
                else
                {
                    correct = true;
                }
            }
        }
        falseSolution:
        if (correct)
        {
            Debug.Log("ColorPuzzle solution is correct");
            door.GetComponent<Door>().OpenDoor();
        }
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
