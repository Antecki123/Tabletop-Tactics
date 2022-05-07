using System.Collections;
using UnityEngine;

public class GridBuilder : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    private Vector2Int gridDimensions;
    private GameObject hexPrefab;
    private Transform gridTransform;

    private int cellsAmount = 0;

    public void Execute(BattlefieldCreator manager)
    {
        gridDimensions.x = manager.mapSizeList[manager.mapSize].x;
        gridDimensions.y = manager.mapSizeList[manager.mapSize].y;
        GridManager.instance.GridDimensions = gridDimensions;

        hexPrefab = manager.hexPrefab;
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
                var offsetY = .75f * j;

                // Create new hex object
                var position = new Vector3(offsetX, 0f, offsetY);
                //var hex = Instantiate(hexPrefab, position, transform.rotation);
                var hex = CreateHexagonMesh();
                hex.transform.position = new Vector3(offsetX, 0f, offsetY);
                hex.transform.SetParent(gridTransform.transform);
                hex.name = $"{i} {j}";

                // Create GridCell component
                var hexComponent = hex.AddComponent<GridCell>();
                hexComponent.Position = new Vector2Int(i, j);

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

                line.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
                //line.material = new Material(Shader.Find("Shader Graphs/Border"));

                line.SetPosition(0, new Vector3(0f, 0f, .5f) + transform.position);
                line.SetPosition(1, new Vector3(.5f, .0f, .25f) + transform.position);
                line.SetPosition(2, new Vector3(.5f, 0f, -.25f) + transform.position);
                line.SetPosition(3, new Vector3(0f, 0f, -.5f) + transform.position);
                line.SetPosition(4, new Vector3(-.5f, 0f, -.25f) + transform.position);
                line.SetPosition(5, new Vector3(-.5f, 0f, .25f) + transform.position);

                line.enabled = true;
                cellsAmount++;
            }
        }
    }

    private GameObject CreateHexagonMesh()
    {
        Vector3[] vertices = new Vector3[7] { new Vector3(0f, 0f, 0f),
                                              new Vector3(0f, 0f, .5f),
                                              new Vector3(.5f, .0f, .25f) ,
                                              new Vector3(.5f, 0f, -.25f) ,
                                              new Vector3(0f, 0f, -.5f),
                                              new Vector3(-.5f, 0f, -.25f),
                                              new Vector3(-.5f, 0f, .25f) };

        Vector2[] uvs = new Vector2[7] { new Vector2(0f, 0f),
                                         new Vector2(0f, .5f),
                                         new Vector2(.5f, .25f),
                                         new Vector2(.5f, -.25f),
                                         new Vector2(0f, -.5f),
                                         new Vector2(-.5f, -.25f),
                                         new Vector2(-.5f, .25f) };

        int[] triangles = new int[18] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 1 };

        Mesh mesh = new();
        mesh.name = "Hexagon";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        var hexObj = new GameObject("hex", typeof(MeshFilter));
        hexObj.GetComponent<MeshFilter>().mesh = mesh;
        hexObj.AddComponent<MeshCollider>().sharedMesh = mesh;

        return hexObj;
    }
}