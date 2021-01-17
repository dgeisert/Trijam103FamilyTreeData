using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    [SerializeField] float speed = 1;
    void Start()
    {
        StartCoroutine(Walk());
    }

    public IEnumerator Walk()
    {
        yield return new WaitForSeconds(5);
        transform.position += speed * new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
        StartCoroutine(Walk());
    }
}
