using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour {

    private Renderer[] ownRenderer;
    public List<GameObject> neighbours;
    private Dictionary<ColorPuzzle, Renderer[]> neighboursRenderer;
    [HideInInspector]
    public Color currentColor;
    private ColorPuzzleBase colorPuzzleBase;
    private bool isFading;

    [HideInInspector]
    public bool finished = false;

    private AudioClip splashSound;
    private AudioClip errorSound;

	void Start () {
        if(GetComponent<Renderer>() == null)
        {
            ownRenderer = GetComponentsInChildren<Renderer>();            //use renderer of children in complex field
        }

        neighboursRenderer = new Dictionary<ColorPuzzle, Renderer[]>();
        foreach (GameObject go in neighbours)
        {
            neighboursRenderer.Add(go.GetComponent<ColorPuzzle>(), go.GetComponentsInChildren<Renderer>());
        }

        colorPuzzleBase = transform.parent.GetComponent<ColorPuzzleBase>();
        splashSound = colorPuzzleBase.splashSound;
        errorSound = colorPuzzleBase.errorSound;
    }

    public void changeColor(Color color)
    {
        bool changeColor = true;

        foreach (Renderer renderer in ownRenderer)
        {
            renderer.material.color = color;            //change color to orb
        }

        foreach (KeyValuePair<ColorPuzzle, Renderer[]> neighbour in neighboursRenderer)
        {
            if (neighbour.Key.currentColor == color)               //if neighbours have same color as hitting orb
            {
                neighbour.Key.resetOwnColor(1);                     //reset color of neighbours back to white
                changeColor = false;
            }
        }

        if (changeColor)
        {
            finished = true;
            colorPuzzleBase.checkSolution();
        }
        else
        {
            colorPuzzleBase.overallFails++;
            resetOwnColor(1);                       //reset own color back to white
            AudioSource.PlayClipAtPoint(errorSound, transform.position, 50);
            colorPuzzleBase.checkForHints();
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        Color color = collision.gameObject.GetComponent<Renderer>().material.color;
        AudioSource.PlayClipAtPoint(splashSound, transform.position);
        if (isFading || color == currentColor)
        {
            Destroy(collision.gameObject);
            return;
        }
        if (!colorPuzzleBase.getIfSolutionIsCorrect() && !isFading)
        {
            currentColor = color;
            changeColor(currentColor);
            Destroy(collision.gameObject);
        }
    }

    public void resetOwnColor(float timer)
    {
        currentColor = Color.white;
        finished = false;
        foreach(Renderer renderer in ownRenderer)
        {
            StartCoroutine(fade(renderer.material, renderer.material.color, Color.white, timer));
        }
    }

    private IEnumerator fade(Material material, Color colorFrom, Color colorTo, float timer)
    {
        isFading = true;
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * (1.0f / timer);
            material.color = Color.Lerp(colorFrom, colorTo, t);
            yield return null;
        }
        isFading = false;
    }
}
