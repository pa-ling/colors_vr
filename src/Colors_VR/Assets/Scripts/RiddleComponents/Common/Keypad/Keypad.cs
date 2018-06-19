using UnityEngine;
using System;

public class Keypad : MonoBehaviour
{

	public string solution = "000";
	public Door door;
	public AudioSource audioClick;
	public AudioSource audioFailure;

	[HideInInspector]
	public bool isSolved = false;
	public event Action OnResetKeyColor;

	private string currentInput = "";

	public void SendCharacter(char character)
	{
		if (!isSolved)
		{
			audioClick.Play();
			currentInput += character;

			if (currentInput.Length == solution.Length)
			{
				if (currentInput == solution)
				{
					isSolved = true;
					door.OpenDoor();
				}
				else
				{
					audioFailure.Play();
					currentInput = "";

					if (OnResetKeyColor != null)
						OnResetKeyColor();
				}
			}
		}
	}
}
