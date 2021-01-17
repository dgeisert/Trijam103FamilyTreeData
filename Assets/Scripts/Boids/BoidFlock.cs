using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlock : MonoBehaviour
{
    [SerializeField] private Boid boid;
    public bool y = false;
    public int randomSpawnCount;
    [SerializeField] private Vector3 randomSpawnRange;
    [SerializeField] private float birthsPerSecond = 1;
    public List<Boid> boids;
    public List<Boid> toRemove;
    public List<Boid> toAdd;
    [SerializeField] private List<BoidAvoid> boidAvoids;
    [SerializeField] private Transform centerMarker;
    [SerializeField] private Flag controlFlag;

    private Color flockColor;
    private Vector3 center;

    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private float avoidRange = 2f;
    [SerializeField] private float visualRange = 15f;
    [SerializeField] private float avoidTurn = 0.05f;
    [SerializeField] private float centerTurn = 0.005f;
    [SerializeField] private float globalCenterBias = 0.0002f;

    [SerializeField] private float avoiderRange = 10;
    [SerializeField] private float avoiderModifier = 2;

    public void Init()
    {
        if (centerMarker == null)
        {
            centerMarker = transform;
        }
        boids = new List<Boid>();
        toRemove = new List<Boid>();
        toAdd = new List<Boid>();

        flockColor = controlFlag != null ? controlFlag.color : new Color(Random.value, Random.value, Random.value, 1);

        //init random boid
        for (int i = 0; i < randomSpawnCount; i++)
        {
            NewBoid(null, null);
        }

        if(controlFlag != null)
        {
            controlFlag.flock = this;
        }
    }

    private void NewBoid(Boid parentA, Boid parentB)
    {
        Boid b = Instantiate(
            boid,
            parentB == null ?
            transform.position + new Vector3(
                (Random.value - 0.5f) * randomSpawnRange.x,
                (Random.value - 0.5f) * randomSpawnRange.y,
                (Random.value - 0.5f) * randomSpawnRange.z) :
            (parentA.transform.position + parentB.transform.position) / 2,
            Quaternion.Euler(0, Random.value * 360, 0),
            transform);
        b.Init(flockColor, parentA, parentB);
        b.flock = this;
        b.dx = Random.value;
        b.dy = y ? Random.value : 0;
        b.dz = Random.value;
        b.speed = speed;
        b.maxSpeed = maxSpeed;
        toAdd.Add(b);
    }

    public void UpdateLists()
    {
        foreach (var b in boids)
        {
            if (b == null)
            {
                toRemove.Add(b);
            }
        }
        foreach (var b in toRemove)
        {
            if (b != null)
            {
                Destroy(b.gameObject);
            }
            boids.Remove(b);
        }
        toRemove = new List<Boid>();
        foreach (var b in toAdd)
        {
            boids.Add(b);
        }
        toAdd = new List<Boid>();
    }

    public void UpdateBoids()
    {
        foreach (var b in boids)
        {
            if (b != null)
            {
                //handle avoids
                foreach (var ba in boidAvoids)
                {
                    b.Avoid(ba.transform.position, avoiderRange, avoidTurn, avoiderModifier);
                }

                float i = 0;
                b.visualNearbyCenter = Vector3.zero;
                foreach (var b2 in boids)
                {
                    if (b2 != null && !b2.dead && b2.flock == this)
                    {
                        float dist = Vector3.Distance(b.xyz, b2.xyz);
                        //if boid is nearby add it to the group you are moving with
                        if (dist < visualRange)
                        {
                            i++;
                            b.visualNearbyCenter += b2.transform.position;
                        }
                        //if boid is very close move away from it
                        if (dist < avoidRange)
                        {
                            b.Avoid(b2.xyz, avoidRange, avoidTurn, 1);
                        }
                    }
                }
                //if there are boid in range move towards the center of them
                if (i > 0)
                {
                    b.visualNearbyCenter /= i;
                    b.dx += (b.visualNearbyCenter.x - b.x) * centerTurn;
                    b.dy += (b.visualNearbyCenter.y - b.y) * centerTurn;
                    b.dz += (b.visualNearbyCenter.z - b.z) * centerTurn;
                }
                else
                {
                    //if alone head towards random member of flock
                    b.dx += (b.x - boids[Mathf.FloorToInt(Random.value * boids.Count)].x) * centerTurn / 2;
                    b.dy += (b.y - boids[Mathf.FloorToInt(Random.value * boids.Count)].y) * centerTurn / 2;
                    b.dz += (b.z - boids[Mathf.FloorToInt(Random.value * boids.Count)].z) * centerTurn / 2;
                }

                //global center bias
                b.dx += (centerMarker.position.x - b.x) * globalCenterBias;
                b.dy += (centerMarker.position.y - b.y) * globalCenterBias;
                b.dz += (centerMarker.position.z - b.z) * globalCenterBias;

                b.Move();
            }
        }
    }

    public void Birth()
    {
        if (boids.Count > 4)
        {
            Boid b = boids.Random1();
            Boid b2 = boids.Random1();
            NewBoid(b, b2);
        }
    }
}