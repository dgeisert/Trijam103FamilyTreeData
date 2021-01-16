using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    void Start()
    {
        transform.localEulerAngles = new Vector3(
            x ? (Random.value * 360) : transform.localEulerAngles.x,
            y ? (Random.value * 360) : transform.localEulerAngles.y,
            z ? (Random.value * 360) : transform.localEulerAngles.z);
    }
}