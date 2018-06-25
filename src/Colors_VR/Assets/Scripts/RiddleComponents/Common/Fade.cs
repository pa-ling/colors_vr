using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    private Renderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(fade(renderer.material, renderer.material.color, collision.gameObject.GetComponent<Renderer>().material.color, 1, false));
        Destroy(collision.gameObject);
    }

    private IEnumerator fade(Material material, Color colorFrom, Color colorTo, float timer, bool paused)
    {
        float t = 0.0f;
        while (t < 1.0 && !paused)
        {
            t += Time.deltaTime * (1.0f / timer);
            material.color = Color.Lerp(colorFrom, colorTo, t);
            paused = !paused;
            yield return null;
        }
    }
}
