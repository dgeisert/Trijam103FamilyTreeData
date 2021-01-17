using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private List<BoidFlock> flocks;
    [SerializeField] private BoidFlock baseFlock;
    [SerializeField] private float range, sizeMult;

    private void Awake()
    {
        if (flocks == null)
        {
            flocks = new List<BoidFlock>();
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            BoidFlock f = transform.GetChild(i).GetComponent<BoidFlock>();
            if (f != null)
            {
                flocks.Add(f);
                f.Init();
            }
        }
    }

    private void Update()
    {
        int fs = 0;
        foreach (var f in flocks)
        {
            if(f.boids.Count > 3)
            {
                fs++;
            }
        }
        if (fs < 3 && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            float dist = 30;
            Vector3 dir = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
            BoidFlock f = GameObject.Instantiate(baseFlock, dist * dir, Quaternion.identity);
            f.randomSpawnCount = flocks[0].boids.Count + Mathf.FloorToInt((Random.value - 0.5f) * 10);
            flocks.Add(f);
            f.Init();
        }
        foreach (var flock in flocks)
        {
            flock.UpdateLists();
        }
        foreach (var flock in flocks)
        {
            flock.UpdateBoids();
        }
    }
}