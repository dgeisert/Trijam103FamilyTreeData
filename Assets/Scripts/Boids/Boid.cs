using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float speed, maxSpeed;
    public float dx, dy, dz;
    public float doMove = 0;
    public bool pauses = false;
    public float x
    {
        get
        {
            return transform.position.x;
        }
    }
    public float y
    {
        get
        {
            return transform.position.y;
        }
    }
    public float z
    {
        get
        {
            return transform.position.z;
        }
    }
    public Vector3 xyz
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    public Vector3 visualNearbyCenter;

    public void Move()
    {
        Vector3 move = new Vector3(dx, dy, dz);
        if (move.magnitude > 5 || doMove >= 0)
        {
            move = Mathf.Min(move.magnitude, maxSpeed) * move.normalized;
            dx = move.x;
            dy = move.y;
            dz = move.z;
            transform.position += move * Time.deltaTime * speed;
            transform.LookAt(transform.position - move);
        }

        //have frequent pauses in the movement instead of being smooth
        if (pauses)
        {
            doMove += Time.deltaTime;
            if (doMove >= 1)
            {
                doMove = -2 * Random.value;
            }
        }
    }

    public void Avoid(Vector3 pos, float range, float turn, float modifier)
    {
        //if avoider is near move away
        float dist = Vector3.Distance(pos, xyz);
        if (dist < range)
        {
            Vector3 dir = (new Vector3(x - pos.x, y - pos.y, z - pos.z)).normalized;
            dx += (range - dist) / range * dir.x * turn * modifier;
            dy += (range - dist) / range * dir.y * turn * modifier;
            dz += (range - dist) / range * dir.z * turn * modifier;
        }
    }
}