using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour {

    public string solution = "000";
    private string currentInput = "";
    private bool isSolved = false;

    public void SendCharacter(char character)
    {
        this.GetComponent<AudioSource>().Play();
        currentInput += character;

        if (currentInput == solution)
        {
            isSolved = true;
            //TODO: Play Succes sound?
            Debug.Log("Puzzle Solved!");
        }

        if (!isSolved && currentInput.Length >= solution.Length)
        {
            Debug.Log(currentInput + " is wrong!");
            currentInput = "";
            //TODO: Play error sound
        }
    }
}
