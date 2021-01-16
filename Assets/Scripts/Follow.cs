using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public float rate = 0.5f;
    public Transform follow;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - follow.position;
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector3(offset.x, 15 + transform.position.z / 80, offset.z);
        transform.position = Vector3.Lerp(transform.position, follow.position + offset, rate);
        transform.LookAt(follow.position);
        //transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rate);
    }
}