using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour {

    private Renderer[] ownRend;
    public List<GameObject> neighbours;

    [HideInInspector]
    public bool finished = false;

    public AudioClip audioClip;

	void Start () {
        if(GetComponent<Renderer>() == null)
        {
            ownRend = GetComponentsInChildren<Renderer>();            //use renderer of children in complex field
        }
        else
        {
            ownRend = new Renderer[] { GetComponent<Renderer>() };
        }
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

                    StartCoroutine(fade(neighbourRender.material, neighbourRender.material.color, Color.white, 2));
                    foreach (Renderer render in ownRend)
                    {
                        StartCoroutine(fade(render.material, render.material.color, Color.white, 2));
                    }
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
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
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
