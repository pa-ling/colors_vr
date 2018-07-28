using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
DecalPool-Controller
Instantiates X number of splat-decals before starting.
Uses only splats from the pool, if all splats are used uses the first used splats (FiFo). 
*/
public class DecalPoolController : MonoBehaviour {

    public int maxDecals;
    public GameObject splatDecal;

    private Queue<GameObject> decalsInPool;
    private Queue<GameObject> decalsInActiveInWorld;

    private void Awake()
    {
        decalsInPool = new Queue<GameObject>();
        decalsInActiveInWorld = new Queue<GameObject>();

        for (int i = 0; i < maxDecals; i++)
        {
            InstantiateDecal();
        }
    }


    private void InstantiateDecal()
    {
        GameObject decal = Instantiate(splatDecal);
        decal.transform.SetParent(transform); ;

        decalsInPool.Enqueue(decal);
        decal.SetActive(false);
    }

    private GameObject GetNextAvailableDecal()
    {
        if (decalsInPool.Count > 0)
            return decalsInPool.Dequeue();

        GameObject oldestActiveDecal = decalsInActiveInWorld.Dequeue();
        return oldestActiveDecal;
    }

    public GameObject SpawnDecal()
    {
        GameObject decal = GetNextAvailableDecal();
        if (decal != null)
        {
            decal.SetActive(true);
            decalsInActiveInWorld.Enqueue(decal);
            return decal;
        }
        Debug.LogError("No decal was found");
        return null;
    }

}
