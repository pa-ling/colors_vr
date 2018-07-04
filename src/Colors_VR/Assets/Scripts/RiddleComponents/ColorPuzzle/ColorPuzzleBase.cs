using UnityEngine;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
    public Door door;

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
			door.OpenDoor();
        }
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
