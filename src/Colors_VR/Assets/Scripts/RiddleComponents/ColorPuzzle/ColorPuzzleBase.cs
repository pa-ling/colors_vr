using System;
using UnityEngine;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
	public int overallFails = 0;
	private bool[] hintsPlayed;
    public Door door;

    private ColorPuzzle[] children;

    public AudioClip splashSound;
    public AudioClip errorSound;

	public Companion companion;

	public AudioClip[] hints;

	[HideInInspector]
	public event Action OnCorrectSolution;

	private void Start()
	{
		hintsPlayed = new bool[hints.Length];

		for (int i = 0; i < hintsPlayed.Length; ++i)
			hintsPlayed[i] = false;

        children = GetComponentsInChildren<ColorPuzzle>();
	}

	public void checkSolution()
    {
        correct = true;
		foreach (ColorPuzzle child in children)
        {
			if (!child.finished)
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("ColorPuzzle solution is correct");
            if (door != null)
            {
                door.OpenDoor();
            }

            if (OnCorrectSolution != null)
            {
                OnCorrectSolution();
            }
        }
    }

    public void checkForHints()
    {
        if (overallFails >= 3 && hints.Length > 0 && !hintsPlayed[0] )
        {
            companion.StartSpeaking(hints[0]);
            hintsPlayed[0] = true;
        }
        else if (overallFails >= 7 && hints.Length > 1 && !hintsPlayed[1])
        {
            companion.StartSpeaking(hints[1]);
            hintsPlayed[1] = true;
        }
        else if (overallFails >= 11 && hints.Length > 2 && !hintsPlayed[2] )
        {
            companion.StartSpeaking(hints[2]);
            hintsPlayed[2] = true;
        }
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
