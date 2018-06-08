using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour {

    private Renderer[] ownRend;
    public List<GameObject> neighbours;

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
                    neighbourRender.material.color = Color.white;
                    foreach (Renderer render in ownRend)
                    {
                        render.material.color = Color.white;                  //fade to white, not instant...
                    }
                    changeColor = false;
                }
            }
        }

        if (changeColor)
        {
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
            Destroy(collision.gameObject);
        }
    }
}
