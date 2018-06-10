using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour {

    private Mesh mesh;
    private Vector3[] verts;
    private Color32[] vertColors;    

	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        vertColors = new Color32[verts.Length];
        Color32 whiteWithZeroAlpha = new Color32(255, 255, 255, 0);
        for (int i = 0; i < verts.Length; i++)
        {
            vertColors[i] = whiteWithZeroAlpha;
        }
        mesh.colors32 = vertColors;
    }

    public void ApplyPaint(Vector3 position, float innerRadius, float outerRadius, Color color)
    {
        Vector3 center = transform.InverseTransformPoint(position);
        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;
        float innerR = innerRadius * outerR / outerRadius;
        float innerRsqr = innerR * innerR;
        float outerRsqr = outerR * outerR;
        float tFactor = 1f / (outerR - innerR);

        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 delta = verts[i] - center;
            float dsqr = delta.sqrMagnitude;
            if (dsqr > outerRsqr)
            {
                continue;
            }
            int a = vertColors[i].a;
            vertColors[i] = color;
            if (dsqr < innerRsqr)
            {
                vertColors[i].a = 255;
            }
            else
            {
                float d = Mathf.Sqrt(dsqr);
                byte blobA = (byte)(255 - 255 * (d - innerR) * tFactor);
                if (blobA >= a)
                {
                    vertColors[i].a = blobA;
                }
            }
            Debug.Log(vertColors[i]);
        }
        mesh.colors32 = vertColors;
    }

}
