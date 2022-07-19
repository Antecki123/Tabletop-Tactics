using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridBuilder : MonoBehaviour, IMapBuilder
{
    public Action OnComplete;

    private readonly Dictionary<SkirmishData.MapSize, Vector2Int> sizeValues = new()
    {
        { SkirmishData.MapSize.XS,   new Vector2Int(10, 10) },
        { SkirmishData.MapSize.S,    new Vector2Int(20, 20) },
        { SkirmishData.MapSize.M,    new Vector2Int(40, 40) },
        { SkirmishData.MapSize.L,    new Vector2Int(80, 80) },
        { SkirmishData.MapSize.XL,   new Vector2Int(100, 100) },
        { SkirmishData.MapSize.XXL,  new Vector2Int(150, 150) },
    };

    private readonly Vector3[] hexagonVerticles = new Vector3[7] { new Vector3(  0f,   0f,      0f),
                                                                   new Vector3(  0f, .07f,  .5774f),
                                                                   new Vector3( .5f, .07f,  .2887f),
                                                                   new Vector3( .5f, .07f, -.2887f),
                                                                   new Vector3(  0f, .07f, -.5774f),
                                                                   new Vector3(-.5f, .07f, -.2887f),
                                                                   new Vector3(-.5f, .07f,  .2887f) };

    [Header("Component References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SkirmishData skirmishData;

    [Header("Grid Settings")]
    [SerializeField] private Transform gridTransform;

    private SkirmishData.MapSize mapSize;

    public void Execute()
    {
        mapSize = skirmishData.mapSize;

        gridManager.GridDimensions = new Vector2Int(sizeValues[mapSize].x, sizeValues[mapSize].y);

        for (int i = 0; i < sizeValues[mapSize].x; i++)
        {
            for (int j = 0; j < sizeValues[mapSize].y; j++)
            {
                GenerateHex(i, j);
            }
        }

        StartCoroutine(WaitForResponse());
    }

    public void Response() => OnComplete?.Invoke();

    private IEnumerator WaitForResponse()
    {
        while (gridManager.GridCellsList.Count != sizeValues[mapSize].x * sizeValues[mapSize].y)
            yield return new WaitForEndOfFrame();

        Debug.Log($"GRID LOADED: {gridManager.GridCellsList.Count} / {sizeValues[mapSize].x * sizeValues[mapSize].y}.");

        Response();
    }

    private async void GenerateHex(int i, int j)
    {
        var offsetX = (j % 2 == 0) ? i : i + .5f;
        var offsetY = .866f * j;

        await Task.Yield();

        // Create new hex object
        var hex = CreateHexagonMesh();
        hex.transform.position = new Vector3(offsetX, 0f, offsetY);
        hex.transform.SetParent(gridTransform.transform);
        hex.layer = LayerMask.NameToLayer("MovementGrid");
        hex.name = $"{i} {j}";

        // Create GridCell component
        var hexComponent = hex.AddComponent<GridCell>();
        hexComponent.Coordinates = new Vector2Int(i, j);

        gridManager.GridCellsList.Add(hexComponent);

        // Create hex boarder LineRenderer
        var line = hex.AddComponent<LineRenderer>();
        line.startWidth = .05f;
        line.endWidth = .05f;
        line.startColor = Color.black;
        line.endColor = Color.black;

        line.loop = true;
        line.positionCount = 6;
        line.useWorldSpace = false;

        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.allowOcclusionWhenDynamic = false;

        line.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        //line.material = new Material(Shader.Find("Shader Graphs/Border"));

        line.SetPosition(0, hexagonVerticles[1] + transform.position);
        line.SetPosition(1, hexagonVerticles[2] + transform.position);
        line.SetPosition(2, hexagonVerticles[3] + transform.position);
        line.SetPosition(3, hexagonVerticles[4] + transform.position);
        line.SetPosition(4, hexagonVerticles[5] + transform.position);
        line.SetPosition(5, hexagonVerticles[6] + transform.position);
        line.enabled = true;

        //Debug.Log($"{Time.realtimeSinceStartup} Hex ({i},{j}) created.");
    }

    private GameObject CreateHexagonMesh()
    {
        Vector3[] vertices = new Vector3[7] { hexagonVerticles[0],
                                              hexagonVerticles[1],
                                              hexagonVerticles[2],
                                              hexagonVerticles[3],
                                              hexagonVerticles[4],
                                              hexagonVerticles[5],
                                              hexagonVerticles[6] };

        int[] triangles = new int[18] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 1 };

        Mesh mesh = new();
        mesh.name = "Hexagon";
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        var hexObj = new GameObject("hex", typeof(MeshFilter));
        hexObj.GetComponent<MeshFilter>().mesh = mesh;
        hexObj.AddComponent<MeshCollider>().sharedMesh = mesh;

        return hexObj;
    }
}