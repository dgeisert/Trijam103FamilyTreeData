using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleIsoMovement : MonoBehaviour
{
    public static SimpleIsoMovement Instance;
    public static Vector3 xyz
    {
        get
        {
            return Instance.transform.position;
        }
    }

    [SerializeField] private float speed = 1;
    [SerializeField] private Transform body;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Controls.Up)
        {
            move += Vector3.forward * Time.deltaTime * speed;
        }
        if (Controls.Down)
        {
            move -= Vector3.forward * Time.deltaTime * speed;
        }
        if (Controls.Left)
        {
            move -= Vector3.right * Time.deltaTime * speed;
        }
        if (Controls.Right)
        {
            move += Vector3.right * Time.deltaTime * speed;
        }
        //look in the direction of movement
        transform.position += move;
        body.LookAt(transform.position - move);
    }
}