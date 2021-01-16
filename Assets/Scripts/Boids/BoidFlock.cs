using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlock : MonoBehaviour
{
    [SerializeField] private Boid boid;
    [SerializeField] private bool y = false;
    [SerializeField] private int randomSpawnCount;
    [SerializeField] private Vector3 randomSpawnRange;
    private List<Boid> boids;
    [SerializeField] private List<BoidAvoid> boidAvoids;
    [SerializeField] private Transform centerMarker;

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
        if(centerMarker == null)
        {
            centerMarker = transform;
        }
        boids = new List<Boid>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Boid b = transform.GetChild(i).GetComponent<Boid>();
            b.dx = Random.value;
            b.dy = y ? Random.value : 0;
            b.dz = Random.value;
            b.speed = speed;
            b.maxSpeed = maxSpeed;
            boids.Add(b);
        }

        //init random boid
        for (int i = 0; i < randomSpawnCount; i++)
        {
            Boid b = Instantiate(
                boid,
                new Vector3(
                    (Random.value - 0.5f) * randomSpawnRange.x,
                    (Random.value - 0.5f) * randomSpawnRange.y,
                    (Random.value - 0.5f) * randomSpawnRange.z),
                Quaternion.Euler(0, Random.value * 360, 0),
                transform);
            b.dx = Random.value;
            b.dy = y ? Random.value : 0;
            b.dz = Random.value;
            b.speed = speed;
            b.maxSpeed = maxSpeed;
            boids.Add(b);
        }
    }

    public void UpdateBoids()
    {
        foreach (var b in boids)
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
            b.dy += (centerMarker.position.y-b.y) * globalCenterBias;
            b.dz += (centerMarker.position.z-b.z) * globalCenterBias;

            b.Move();
        }
    }
}