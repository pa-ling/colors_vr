using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour {

    private Renderer[] ownRend;
    public List<GameObject> neighbours;

    [HideInInspector]
    public bool finished = false;
	[HideInInspector]
	public int fails = 0;

    private AudioClip splashSound;
    private AudioClip errorSound;

	void Start () {
        if(GetComponent<Renderer>() == null)
        {
            ownRend = GetComponentsInChildren<Renderer>();            //use renderer of children in complex field
        }
        else
        {
            ownRend = new Renderer[] { GetComponent<Renderer>() };
        }

        splashSound = transform.parent.GetComponent<ColorPuzzleBase>().splashSound;
        errorSound = transform.parent.GetComponent<ColorPuzzleBase>().errorSound;
    }

    public void changeColor(Color color)
    {
        bool changeColor = true;
        Renderer[] neighbourRend;

        foreach (GameObject go in neighbours)
        {
            if (go.GetComponent<Renderer>() == null)
            {
                neighbourRend = go.GetComponentsInChildren<Renderer>();
            }
            else
            {
                neighbourRend = new Renderer[] { go.GetComponent<Renderer>() };
            }

            foreach (Renderer neighbourRender in neighbourRend)
            {
                if (neighbourRender.material.color == color)
                {
                    finished = false;
                    go.GetComponent<ColorPuzzle>().finished = false;
                    foreach (Renderer render in ownRend)
                    {
                        render.material.color = color;
                    }

                    StartCoroutine(fade(neighbourRender.material, neighbourRender.material.color, Color.white, 1));
                    foreach (Renderer render in ownRend)
                    {
                        StartCoroutine(fade(render.material, render.material.color, Color.white, 1));
                    }
                    AudioSource.PlayClipAtPoint(errorSound, transform.position, 50);
					++fails;
					changeColor = false;
                }
            }
        }

        if (changeColor)
        {
            finished = true;
            foreach(Renderer render in ownRend)
            {
                render.material.color = color;
            }
        }

        transform.parent.GetComponent<ColorPuzzleBase>().checkSolution();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!transform.parent.GetComponent<ColorPuzzleBase>().getIfSolutionIsCorrect())
        {
            changeColor(collision.gameObject.GetComponent<Renderer>().material.color);
            AudioSource.PlayClipAtPoint(splashSound, transform.position);
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator fade(Material material, Color colorFrom, Color colorTo, float timer)
    {
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * (1.0f / timer);
            material.color = Color.Lerp(colorFrom, colorTo, t);
            yield return null;
        }
    }
}
