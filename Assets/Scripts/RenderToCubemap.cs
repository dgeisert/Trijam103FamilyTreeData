using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenderToCubemap : MonoBehaviour
{
    [SerializeField] Cubemap cubemap;
    [SerializeField] Camera cam;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cam.RenderToCubemap(cubemap);
        }
    }
}