using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolorMesh : MonoBehaviour
{
    [SerializeField] private Color color;
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            Color[] colors = new Color[mf.mesh.vertices.Length];
            for (int i = 0; i < mf.mesh.vertices.Length; i++)
            {
                colors[i] = color;
            }
            mf.mesh.colors = colors;
        }
    }
}