using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    [SerializeField] private float dist;
    void Start()
    {
        transform.localPosition += new Vector3(
            x ? (dist * (Random.value - 0.5f) * 2) : transform.localPosition.x,
            y ? (dist * (Random.value - 0.5f) * 2) : transform.localPosition.y,
            z ? (dist * (Random.value - 0.5f) * 2) : transform.localPosition.z);
    }
}