using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePhysicsObject : MonoBehaviour {

    private Renderer renderer;
    public float timerForFade;
    public bool shouldTeleportOrbVanish;
    public bool canTeleport;

	void Start () {
        renderer = GetComponent<Renderer>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PhysicsOrb>() != null)
        {
            StartCoroutine(fade(renderer.material, renderer.material.color, collision.gameObject.GetComponent<Renderer>().material.color, timerForFade, false));
        }

        if (collision.gameObject.GetComponent<TeleportOrb>() != null && shouldTeleportOrbVanish)
        {
            if (canTeleport)
            {
                StartCoroutine(collision.gameObject.GetComponent<TeleportOrb>().Teleport(new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), 0.2f)); 
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
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
