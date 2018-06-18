using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour {

    private Mesh mesh;
    private Vector3[] verts;
    private Color32[] vertColors;
    private Color32[] paints = {new Color32(255, 0, 0, 0), new Color32(0, 255, 0, 0), new Color32(0, 0, 255, 0), new Color32(0, 0, 0, 255)};

	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        vertColors = new Color32[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            vertColors[i] = Color.clear;
        }
        mesh.colors32 = vertColors;
    }

    public void ApplyPaint(Vector3 position, float innerRadius, float outerRadius, int colorChannelIndex)
    {
        Vector3 center = transform.InverseTransformPoint(position);
        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;
        float innerR = innerRadius * outerR / outerRadius;
        float innerRsqr = innerR * innerR;
        float outerRsqr = outerR * outerR;
        float tFactor = 1f / (outerR - innerR);

        Vector3 delta;
        float dsqr;
        byte[] currentColor = {0,0,0,0};
        for (int i = 0; i < verts.Length; i++)
        {
            delta = verts[i] - center;
            dsqr = delta.sqrMagnitude;
            Color32ToArray(vertColors[i], currentColor);
            if (dsqr > outerRsqr) //outside of the splash
            {
                continue;
            }
            if (dsqr >= innerRsqr) //edge of the splash
            {
                float d = Mathf.Sqrt(dsqr);
                byte blob = (byte)(255 - 255 * (d - innerR) * tFactor);

                //KeyValuePair<int, byte> currentHighestChannel = GetHighestChannel(currentColor);

                if (currentColor[colorChannelIndex] < blob)
                {
                    ZeroArray(currentColor);
                    currentColor[colorChannelIndex] = blob;
                    vertColors[i] = ArrayToColor32(currentColor);
                }
            }
            else //inside the splash
            {
                vertColors[i] = paints[colorChannelIndex];
            }
            Debug.Log(vertColors[i]);
        }
        mesh.colors32 = vertColors;
    }

    private KeyValuePair<int, byte> GetHighestChannel(byte[] color)
    {
        KeyValuePair<int, byte> highestChannel = new KeyValuePair<int, byte>(int.MinValue, 0);

        for (int i = 0; i < color.Length; i++)
        {
            if (color[i] > highestChannel.Value)
            {
                highestChannel = new KeyValuePair<int, byte>(i, color[i]);
            }
        }

        return highestChannel;
    }

    private void Color32ToArray(Color32 c, byte[] a)
    {
        a[0] = c.r;
        a[1] = c.g;
        a[2] = c.b;
        a[3] = c.a;
    }

    private void ZeroArray(byte[]a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 0;
        }
    }

    private Color32 ArrayToColor32(byte[] a)
    {
        return new Color32(a[0], a[1], a[2], a[3]);
    }

}
