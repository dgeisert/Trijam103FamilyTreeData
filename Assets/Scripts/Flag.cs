using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public static Flag Instance;
    public Color color;
    [SerializeField] RecolorMesh[] recolor;
    [SerializeField] Camera cam;
    [SerializeField] float speed = 1;
    int foo = 0;
    public BoidFlock flock;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        color = new Color(Random.value, Random.value, Random.value, 1);
        foreach (var item in recolor)
        {
            item.Recolor(color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foo++;
        if (flock.boids.Count == 0 && foo > 4)
        {
            Game.Instance.Victory();
        }
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
        cam.transform.position += move;
        if (Controls.Click)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 99, 1 << 8))
            {
                transform.position = hit.point;
            }
            else
            {
                Debug.Log("Didn't hit land with click raycast");
            }
        }
    }
}