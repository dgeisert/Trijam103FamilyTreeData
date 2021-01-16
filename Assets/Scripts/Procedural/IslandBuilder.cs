using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class IslandLevel
{
    [SerializeField] public Color color;
    [SerializeField] public GameObject[] objects;
    [SerializeField] public float objectRate;
}

public class IslandBuilder : MonoBehaviour
{
    public Material islandMat;
    public int size = 40;
    public float scale = 1;
    public float height = 5;
    public float perlinScale = 0.1f;

    [SerializeField] public IslandLevel[] levels;

    void Start()
    {
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> verts2 = new List<Vector3>();
        List<Color> vertColors = new List<Color>();
        List<int> tris = new List<int>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float y =
                    Mathf.PerlinNoise(transform.position.x + i * perlinScale, transform.position.z + j * perlinScale) *
                    Mathf.PerlinNoise(transform.position.x + i * perlinScale / 2, transform.position.z + j * perlinScale / 2) *
                    Mathf.PerlinNoise(transform.position.x + i * perlinScale / 8, transform.position.z + j * perlinScale / 8) *
                    height *
                    (1 - 8f * Mathf.Sqrt((Mathf.Pow(Mathf.Abs(i - (float) size / 2f) / (float) size, 2) + Mathf.Pow(Mathf.Abs(j - (float) size / 2f) / (float) size, 2))));

                if (y > -1 && (j == 0 || i == 0 || j == size - 1 || i == size - 1))
                {
                    y = -1;
                }

                verts.Add(scale * new Vector3(i + Random.value, y, j + Random.value));
                if (i > 0 && j > 0 && (
                        verts[(i - 1) * size + j - 1].y > -scale ||
                        verts[(i - 1) * size + j].y > -scale ||
                        verts[i * size + j - 1].y > -scale ||
                        verts[i * size + j].y > -scale))
                {

                    Color tri1Color = levels[new int[]
                    {
                        Area(verts[(i - 1) * size + j - 1].y),
                            Area(verts[(i - 1) * size + j].y),
                            Area(verts[i * size + j - 1].y)
                    }.Max()].color;
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);

                    verts2.Add(verts[(i - 1) * size + j - 1] + Vector3.up * Area(verts[(i - 1) * size + j - 1].y * scale));
                    verts2.Add(verts[(i - 1) * size + j] + Vector3.up * Area(verts[(i - 1) * size + j].y * scale));
                    verts2.Add(verts[i * size + j - 1] + Vector3.up * Area(verts[i * size + j - 1].y * scale));

                    tris.Add(verts2.Count - 3);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 1);

                    Color tri2Color = levels[new int[]
                    {
                        Area(verts[i * size + j].y),
                            Area(verts[(i - 1) * size + j].y),
                            Area(verts[i * size + j - 1].y)
                    }.Max()].color;

                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);

                    verts2.Add(verts[i * size + j] + Vector3.up * Area(verts[i * size + j].y * scale));
                    verts2.Add(verts[(i - 1) * size + j] + Vector3.up * Area(verts[(i - 1) * size + j].y * scale));
                    verts2.Add(verts[i * size + j - 1] + Vector3.up * Area(verts[i * size + j - 1].y * scale));

                    tris.Add(verts2.Count - 1);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 3);

                    if (levels != null && levels.Length >= Area(verts[i * size + j].y) && levels[Area(verts[i * size + j].y)].objects.Length > 0 &&
                        verts[i * size + j].y > -scale && levels[Area(verts[i * size + j].y)].objectRate > Random.value * Mathf.PerlinNoise(-transform.position.x + i * perlinScale * 2, -transform.position.z + j * perlinScale * 2))
                    {
                        GameObject.Instantiate(RandomObject(levels[Area(verts[i * size + j].y)]),
                            transform.position + verts2[verts2.Count - 3],
                            Quaternion.Euler(0, Random.value * 360, 0),
                            transform);
                    }
                }
            }
        }

        mesh.vertices = verts2.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = vertColors.ToArray();
        mesh.RecalculateNormals();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = islandMat;
        gameObject.AddComponent<MeshCollider>();
    }

    int Area(float height)
    {
        return Mathf.Clamp((int) ((height / scale + 1f) * 2f), 0, levels.Length - 1);
    }

    GameObject RandomObject(IslandLevel objectList)
    {
        return objectList.objects[Mathf.FloorToInt(Random.value * objectList.objects.Length)];
    }

    void GenerateMesh() { }
}