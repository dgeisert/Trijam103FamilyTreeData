using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] int count;
    [SerializeField] GameObject toSpawn;
    [SerializeField] Vector2 range;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject.Instantiate(toSpawn, new Vector3(range.x * (Random.value - 0.5f), 0, range.y * (Random.value - 0.5f)), Quaternion.identity, transform);
        }
    }
}