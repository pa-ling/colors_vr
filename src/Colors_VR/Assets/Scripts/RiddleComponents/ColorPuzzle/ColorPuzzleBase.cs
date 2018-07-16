using System;
using UnityEngine;

public class ColorPuzzleBase : MonoBehaviour {

    private bool correct = false;
	private int overallFails = 0;
	private bool[] hintsPlayed;
    public Door door;

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
	}

	public void checkSolution()
    {
		correct = true;

		foreach (Transform child in transform)
        {
			ColorPuzzle colorPuzzle = child.GetComponent<ColorPuzzle>();

			overallFails += colorPuzzle.fails;

			if (!colorPuzzle.finished)
            {
                correct = false;
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
		else
		{
			Debug.Log(overallFails);
			if (overallFails >= 3 && !hintsPlayed[0] && hints.Length > 0)
			{
				companion.StartSpeaking(hints[0]);
				hintsPlayed[0] = true;
			}
			else if (overallFails >= 6 && !hintsPlayed[1] && hints.Length > 1)
			{
				companion.StartSpeaking(hints[1]);
				hintsPlayed[1] = true;
			}
			else if (overallFails >= 9 && !hintsPlayed[2] && hints.Length > 2)
			{
				companion.StartSpeaking(hints[2]);
				hintsPlayed[2] = true;
			}
		}
    }

    public bool getIfSolutionIsCorrect()
    {
        return correct;
    }
}
