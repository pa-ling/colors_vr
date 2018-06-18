using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
    public Door door;
	public NavMeshLink navMeshLink;

    public void checkSolution()
    {
        foreach (Transform child in transform)
        {
            if (!child.GetComponent<ColorPuzzle>().finished)
            {
                correct = false;
                goto falseSolution;
            }
            else
            {
                correct = true;
            }

        
        }
        falseSolution:
        if (correct)
        {
            Debug.Log("ColorPuzzle solution is correct");
			navMeshLink.area = 0;
			door.OpenDoor();
        }
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
