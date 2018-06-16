using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
    public Door door;
	public NavMeshLink navMeshLink;

    public AudioClip audioClip;

    public void Awake()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<ColorPuzzle>().audioClip = audioClip;
        }
    }

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
			navMeshLink.area = 0;
			door.OpenDoor();
        }
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
