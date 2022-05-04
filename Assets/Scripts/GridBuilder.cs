using System.Collections;
using UnityEngine;

public class GridBuilder : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    private Vector2Int gridDimensions;
    private GameObject hexPrefab;
    private Transform gridTransform;

    private int gridAmount = 0;

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
        while (gridAmount < gridDimensions.x * gridDimensions.y)
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
                var hex = Instantiate(hexPrefab, position, Quaternion.Euler(90f, 0f, 0f));
                hex.transform.SetParent(gridTransform.transform);
                hex.name = $"{i} {j}";

                var hexComponent = hex.GetComponent<GridNode>();
                hexComponent.Position = new Vector2Int(i, j);

                GridManager.instance.GridNodes.Add(hexComponent);

                // Create hex boarder LineRenderer
                var line = hex.AddComponent<LineRenderer>();
                line.startWidth = .025f;
                line.loop = true;
                line.positionCount = 6;
                line.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));

                line.SetPosition(0, new Vector3(0f + offsetX, 0f, .5f + offsetY) + transform.position);
                line.SetPosition(1, new Vector3(.5f + offsetX, 0f, .25f + offsetY) + transform.position);
                line.SetPosition(2, new Vector3(.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(3, new Vector3(0f + offsetX, 0f, -.5f + offsetY) + transform.position);
                line.SetPosition(4, new Vector3(-.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(5, new Vector3(-.5f + offsetX, 0f, .25f + offsetY) + transform.position);

                line.enabled = true;
                gridAmount++;
            }
        }
    }
}