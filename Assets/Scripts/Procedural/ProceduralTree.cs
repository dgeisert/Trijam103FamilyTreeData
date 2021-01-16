using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTree : MonoBehaviour
{
    [SerializeField] bool grow;
    [SerializeField] private Material trunkMaterial, leafMaterial;

    [SerializeField] private int rootBalls = 5, mainBranches = 5, secondaryBranches = 5;
    [SerializeField] private Color woodLow, woodHigh, leafLow, leafHigh;
    private float growth = 1f;
    private int seed;

    private MeshRenderer mrWood, mrLeaf;
    private MeshFilter mfWood, mfLeaf;

    private void Start()
    {
        seed = Mathf.FloorToInt(Random.value * 500);
        if (grow)
        {
            growth = 0.1f;
            StartCoroutine(Grow());
        }
        else
        {
            growth = (Random.value * 0.5f);
        }
        Init(seed);
    }

    private IEnumerator Grow()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        growth += 0.01f;
        Init(seed);
        StartCoroutine(Grow());
    }

    public void Init(int seed)
    {
        Mesh mesh = mfWood?.mesh != null ? mfWood.mesh : new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Color> vertColors = new List<Color>();
        List<int> tris = new List<int>();

        Mesh mesh2 = mfLeaf?.mesh != null ? mfLeaf.mesh : new Mesh();
        List<Vector3> verts2 = new List<Vector3>();
        List<Color> vertColors2 = new List<Color>();
        List<int> tris2 = new List<int>();

        Random.InitState(seed);

        Color wood = Color.Lerp(woodLow, woodHigh, Random.value);
        Color leaves = Color.Lerp(leafLow, leafHigh, Random.value);

        Vector3 center = Vector3.zero;
        Vector2 scale = growth * new Vector2(1, 4);
        //main trunk
        AddRect(center, scale, 0, wood, verts, vertColors, tris);

        //main branches
        int val = Mathf.FloorToInt(Random.value * mainBranches) + 1;
        int[] seeds = new int[val + 1];
        for (int i = 0; i <= val; i++)
        {
            seeds[i] = Mathf.FloorToInt(1000 * Random.value);
        }
        for (int i = 0; i < val; i++)
        {
            AddBranch(center + Vector3.up * (1 - seeds[i] / 2000) * scale.y,
                0.6f * scale,
                i % 4 + 1,
                seeds[i],
                wood, leaves,
                verts, vertColors, tris,
                verts2, vertColors2, tris2);
        }

        AddBranch(center + Vector3.up * (scale.y - 0.6f * scale.x / 2),
            0.6f * scale,
            Mathf.FloorToInt(seeds[val] / 250) + 1,
            seeds[val],
            wood, leaves,
            verts, vertColors, tris,
            verts2, vertColors2, tris2);

        for (int i = 0; i < val; i++)
        {
            if (seeds[i] / 100 < growth)
            {
                AddRect(
                    new Vector3(seeds[i] / 1000f - 0.5f, -seeds[i] / 1000f, seeds[i] / 1000f - 0.5f) * scale.x,
                    new Vector2(scale.x, scale.x),
                    0,
                    wood,
                    verts, vertColors, tris);
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = vertColors.ToArray();
        mesh.RecalculateNormals();

        if (mfWood == null)
        {
            mfWood = gameObject.AddComponent<MeshFilter>();
            mfWood.mesh = mesh;
            mrWood = gameObject.AddComponent<MeshRenderer>();
            mrWood.material = trunkMaterial;
        }

        mesh2.vertices = verts2.ToArray();
        mesh2.triangles = tris2.ToArray();
        mesh2.colors = vertColors2.ToArray();
        mesh2.RecalculateNormals();

        if (mfLeaf == null)
        {
            GameObject go = new GameObject("Leaves");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;

            mfLeaf = go.AddComponent<MeshFilter>();
            mfLeaf.mesh = mesh2;
            mrLeaf = go.AddComponent<MeshRenderer>();
            mrLeaf.material = leafMaterial;
            mrLeaf.receiveShadows = false;
        }
    }

    public void AddBranch(Vector3 center, Vector2 scale, int dir, int seed, Color wood, Color leaves, List<Vector3> verts, List<Color> vertColors, List<int> tris, List<Vector3> verts2, List<Color> vertColors2, List<int> tris2)
    {
        Random.InitState(seed);
        center += Vector3.one * (Random.value - 0.5f) * scale.x / 2;
        AddRect(center, scale, dir, wood, verts, vertColors, tris);

        if (scale.x > Random.value + 0.2f)
        {
            switch (dir)
            {
                case 1:
                    AddBranch(
                        center + Vector3.right * (1 - Random.value / 2) * scale.y,
                        scale * 0.5f,
                        Random.value > 0.5f ? 2 : 4,
                        Mathf.FloorToInt(Random.value * 1000),
                        wood, leaves,
                        verts, vertColors, tris,
                        verts2, vertColors2, tris2);
                    break;
                case 2:
                    AddBranch(
                        center + Vector3.forward * (1 - Random.value / 2) * scale.y,
                        scale * 0.5f,
                        Random.value > 0.5f ? 1 : 3,
                        Mathf.FloorToInt(Random.value * 1000),
                        wood, leaves,
                        verts, vertColors, tris,
                        verts2, vertColors2, tris2);
                    break;
                case 3:
                    AddBranch(
                        center - Vector3.right * (1 - Random.value / 2) * scale.y,
                        scale * 0.5f,
                        Random.value > 0.5f ? 2 : 4,
                        Mathf.FloorToInt(Random.value * 1000),
                        wood, leaves,
                        verts, vertColors, tris,
                        verts2, vertColors2, tris2);
                    break;
                case 4:
                    AddBranch(
                        center - Vector3.forward * (1 - Random.value / 2) * scale.y,
                        scale * 0.5f,
                        Random.value > 0.5f ? 1 : 3,
                        Mathf.FloorToInt(Random.value * 1000),
                        wood, leaves,
                        verts, vertColors, tris,
                        verts2, vertColors2, tris2);
                    break;
                default:
                    break;
            }
        }

        {
            Vector3 c2 = center + (dir < 3 ? 1 : -1) * (dir % 2 == 0 ? Vector3.forward : Vector3.right) * (scale.y - scale.x / 2);
            AddRect(
                c2,
                scale * 0.99f,
                0,
                wood,
                verts, vertColors, tris);
            AddRect(
                c2 + Vector3.up * scale.y,
                new Vector2(scale.y, scale.y),
                0,
                leaves,
                verts2, vertColors2, tris2);
        }
    }

    //dir 0-up, 1-right, 2-forward, 3-left, 4-back
    public void AddRect(Vector3 center, Vector2 scale, int dir, Color col, List<Vector3> verts, List<Color> vertColors, List<int> tris)
    {
        if (dir == 0)
        {
            verts.Add(center + new Vector3(scale.x / 2, 0, scale.x / 2));
            verts.Add(center + new Vector3(-scale.x / 2, 0, scale.x / 2));
            verts.Add(center + new Vector3(-scale.x / 2, 0, -scale.x / 2));
            verts.Add(center + new Vector3(scale.x / 2, 0, -scale.x / 2));
            verts.Add(center + new Vector3(scale.x / 2, scale.y, scale.x / 2));
            verts.Add(center + new Vector3(-scale.x / 2, scale.y, scale.x / 2));
            verts.Add(center + new Vector3(-scale.x / 2, scale.y, -scale.x / 2));
            verts.Add(center + new Vector3(scale.x / 2, scale.y, -scale.x / 2));
        }
        else if (dir == 1)
        {
            verts.Add(center + new Vector3(0, scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(0, -scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(0, -scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(0, scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(scale.y, scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(scale.y, -scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(scale.y, -scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(scale.y, scale.x / 2, scale.x / 2));
        }
        else if (dir == 2)
        {
            verts.Add(center + new Vector3(scale.x / 2, -scale.x / 2, 0));
            verts.Add(center + new Vector3(-scale.x / 2, -scale.x / 2, 0));
            verts.Add(center + new Vector3(-scale.x / 2, scale.x / 2, 0));
            verts.Add(center + new Vector3(scale.x / 2, scale.x / 2, 0));
            verts.Add(center + new Vector3(scale.x / 2, -scale.x / 2, scale.y));
            verts.Add(center + new Vector3(-scale.x / 2, -scale.x / 2, scale.y));
            verts.Add(center + new Vector3(-scale.x / 2, scale.x / 2, scale.y));
            verts.Add(center + new Vector3(scale.x / 2, scale.x / 2, scale.y));
        }
        else if (dir == 3)
        {
            verts.Add(center + new Vector3(0, scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(0, -scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(0, -scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(0, scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(-scale.y, scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(-scale.y, -scale.x / 2, scale.x / 2));
            verts.Add(center + new Vector3(-scale.y, -scale.x / 2, -scale.x / 2));
            verts.Add(center + new Vector3(-scale.y, scale.x / 2, -scale.x / 2));
        }
        else if (dir == 4)
        {
            verts.Add(center + new Vector3(scale.x / 2, scale.x / 2, 0));
            verts.Add(center + new Vector3(-scale.x / 2, scale.x / 2, 0));
            verts.Add(center + new Vector3(-scale.x / 2, -scale.x / 2, 0));
            verts.Add(center + new Vector3(scale.x / 2, -scale.x / 2, 0));
            verts.Add(center + new Vector3(scale.x / 2, scale.x / 2, -scale.y));
            verts.Add(center + new Vector3(-scale.x / 2, scale.x / 2, -scale.y));
            verts.Add(center + new Vector3(-scale.x / 2, -scale.x / 2, -scale.y));
            verts.Add(center + new Vector3(scale.x / 2, -scale.x / 2, -scale.y));
        }

        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);
        vertColors.Add(col);

        int i = verts.Count - 8;

        tris.Add(i);
        tris.Add(i + 1);
        tris.Add(i + 2);

        tris.Add(i + 2);
        tris.Add(i + 3);
        tris.Add(i);

        tris.Add(i + 1);
        tris.Add(i);
        tris.Add(i + 4);

        tris.Add(i + 1);
        tris.Add(i + 4);
        tris.Add(i + 5);

        tris.Add(i + 2);
        tris.Add(i + 1);
        tris.Add(i + 5);

        tris.Add(i + 2);
        tris.Add(i + 5);
        tris.Add(i + 6);

        tris.Add(i + 3);
        tris.Add(i + 2);
        tris.Add(i + 6);

        tris.Add(i + 3);
        tris.Add(i + 6);
        tris.Add(i + 7);

        tris.Add(i);
        tris.Add(i + 3);
        tris.Add(i + 7);

        tris.Add(i);
        tris.Add(i + 7);
        tris.Add(i + 4);

        tris.Add(i + 5);
        tris.Add(i + 4);
        tris.Add(i + 6);

        tris.Add(i + 7);
        tris.Add(i + 6);
        tris.Add(i + 4);
    }
}