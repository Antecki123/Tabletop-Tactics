using System.Collections;
using UnityEngine;

public class GridBuilder : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    private Vector2Int gridDimensions;
    private Transform gridTransform;

    private readonly Vector3[] hexagonVerticles = new Vector3[7] { new Vector3(  0f,   0f,      0f),
                                                                   new Vector3(  0f, .07f,  .5774f),
                                                                   new Vector3( .5f, .07f,  .2887f),
                                                                   new Vector3( .5f, .07f, -.2887f),
                                                                   new Vector3(  0f, .07f, -.5774f),
                                                                   new Vector3(-.5f, .07f, -.2887f),
                                                                   new Vector3(-.5f, .07f,  .2887f) };

    private int cellsAmount = 0;

    public void Execute(BattlefieldCreator manager)
    {
        gridDimensions.x = manager.mapSizeList[manager.mapSize].x;
        gridDimensions.y = manager.mapSizeList[manager.mapSize].y;
        GridManager.instance.GridDimensions = gridDimensions;

        gridTransform = manager.gridTransform;

        GenerateHexagonalGrid();
        StartCoroutine(WaitForBulidGrid());
    }

    public void Response() => OnComplete?.Invoke();

    private IEnumerator WaitForBulidGrid()
    {
        while (cellsAmount < gridDimensions.x * gridDimensions.y)
            yield return new WaitForEndOfFrame();

        Debug.Log($"Grid Loaded ({gridDimensions.x}x{gridDimensions.y})");
        Response();
    }

    private void GenerateHexagonalGrid()
    {
        for (int i = 0; i < gridDimensions.x; i++)
        {
            for (int j = 0; j < gridDimensions.y; j++)
            {
                var offsetX = (j % 2 == 0) ? i : i + .5f;
                var offsetY = .866f * j;

                // Create new hex object
                var hex = CreateHexagonMesh();
                hex.transform.position = new Vector3(offsetX, 0f, offsetY);
                hex.transform.SetParent(gridTransform.transform);
                hex.layer = LayerMask.NameToLayer("MovementGrid");
                hex.name = $"{i} {j}";

                // Create GridCell component
                var hexComponent = hex.AddComponent<GridCell>();
                hexComponent.Coordinates = new Vector2Int(i, j);

                GridManager.instance.GridNodes.Add(hexComponent);   // TODO: moze dodac ref zamiast singleton?

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
                cellsAmount++;
            }
        }
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