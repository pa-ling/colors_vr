using UnityEngine;


/*
Class for Z-Fighting (flickering of 2 gameobjects when they have the same position -> both gameobjects want to get rendered first.
Use with minimal distance to other gameobjects (0.00002) and minimal distance to each other (0.00001) so flickering is minimal
When reaching 100 000 splats the distance to other gameobjects will be visible (1 unit).
*/
public class ZFighting : MonoBehaviour {

    public float ZFightingDistanceStart;
    public float ZFightingDistanceStep;
}
