using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    [SerializeField] private float min = 1, max = 1;
    void Start()
    {
        float val = Random.value;
        transform.localScale = new Vector3(
            x ? (val * (max - min) + min) : transform.localScale.x,
            y ? (val * (max - min) + min) : transform.localScale.y,
            z ? (val * (max - min) + min) : transform.localScale.z);
    }
}