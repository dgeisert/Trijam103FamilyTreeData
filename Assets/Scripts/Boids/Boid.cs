using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float speed, maxSpeed;
    public float dx, dy, dz;
    public float doMove = 0;
    public bool pauses = false;
    public Color color, skinColor;
    public BoidFlock flock;
    [SerializeField] Color skinA, skinB;
    public RecolorMesh[] clothes, skin;
    public float age, health, strength, agility, intelligence;
    public bool dead;
    public Boid target;
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

    public void Take(Color setColor, BoidFlock newFlock)
    {
        color = setColor;
        flock.toRemove.Add(this);
        flock = newFlock;
        dead = true;
        newFlock.toAdd.Add(this);
        foreach (var item in clothes)
        {
            item.Recolor(color);
        }
        health = strength;
    }

    public void Init(Color setColor, Boid parentA, Boid parentB)
    {
        color = setColor;
        foreach (var item in clothes)
        {
            item.Recolor(color);
        }
        if (parentA == null)
        {
            skinColor = Color.Lerp(skinA, skinB, Random.value);
            age = Random.value * 40;
            strength = Random.value * 0.5f + 0.75f;
            agility = Random.value * 0.5f + 0.75f;
            intelligence = Random.value * 0.5f + 0.75f;
            transform.localScale = Vector3.one * strength * (0.3f + age / 18 * 0.7f);
            health = strength;
        }
        else
        {
            skinColor = new Color(
                Mathf.LerpUnclamped(parentA.skinColor.r, parentB.skinColor.r, Random.value * 1.5f - 0.25f),
                Mathf.LerpUnclamped(parentA.skinColor.g, parentB.skinColor.g, Random.value * 1.5f - 0.25f),
                Mathf.LerpUnclamped(parentA.skinColor.b, parentB.skinColor.b, Random.value * 1.5f - 0.25f),
                1
            );
            age = 1;
            strength = Mathf.LerpUnclamped(parentA.strength, parentB.strength, Random.value * 1.5f - 0.25f);
            agility = Mathf.LerpUnclamped(parentA.agility, parentB.agility, Random.value * 1.5f - 0.25f);
            intelligence = Mathf.LerpUnclamped(parentA.intelligence, parentB.intelligence, Random.value * 1.5f - 0.25f);
        }
        foreach (var item in skin)
        {
            item.Recolor(skinColor);
        }
    }

    public void Move()
    {
        if (dead)
        {
            transform.position -= Vector3.up * Time.deltaTime / 4;
            transform.eulerAngles += Vector3.right / 4;
            if (transform.position.y < -1)
            {
                flock.toRemove.Add(this);
            }
            return;
        }
        age += Time.deltaTime;
        transform.localScale = Vector3.one * strength * (0.3f + Mathf.Min(age, 18) / 18 * 0.7f);

        if (age > 55)
        {
            bool oldAgeDeath = health > 0;
            health -= Random.value < 0.1f ? Time.deltaTime : 0;
            if (oldAgeDeath && health < 0)
            {
                flock.Birth();
            }
        }
        CheckHealth();

        if (target != null)
        {
            if (target.dead)
            {
                target = null;
            }
            else if (target.flock == flock)
            {
                target = null;
            }
            else
            {
                if (Vector3.Distance(xyz, target.xyz) > 1)
                {
                    Vector3 dir = target.xyz - xyz;
                    xyz = new Vector3(x, 0, z);
                    xyz += dir.normalized * Time.deltaTime * maxSpeed * agility * (health / 4 + 0.75f);
                    transform.LookAt(xyz + dir);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else
                {
                    bool kill = (target.health > 0);
                    target.health -= transform.localScale.x * Time.deltaTime / 3;
                    if (kill && target.health < 0)
                    {
                        target.Take(color, flock);
                        Game.Score += 100;
                    }
                    xyz = new Vector3(x, Mathf.Abs(Mathf.Sin(Time.time * 9)) / 3, z);
                }
                return;
            }
        }

        Vector3 move = new Vector3(dx, dy, dz);
        if (move.magnitude > 5 || doMove >= 0)
        {
            move = Mathf.Min(move.magnitude, maxSpeed) * move.normalized;
            dx = move.x;
            dy = move.y;
            dz = move.z;
            xyz += move * Time.deltaTime * speed * agility * (health / 4 + 0.75f);
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
        if (!flock.y)
        {
            xyz = new Vector3(x, 0, z);
        }
    }

    public void CheckHealth()
    {
        if (health < 0)
        {
            Die();
            return;
        }
    }

    public void Die()
    {
        dead = true;
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

    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.gameObject.GetComponent<Boid>();
        if (b != null && b.flock != flock && (target == null || target.dead))
        {
            target = b;
        }
    }
}