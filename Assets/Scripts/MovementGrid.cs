using System.Collections.Generic;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    [Header("Grid Properties")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [Space]
    [SerializeField] private Material nodeMaterial;
    [SerializeField] private GameObject hexPrefab;
    [Space]
    [SerializeField] private List<GridNode> gridNodes = new();

    private void Start() => GenerateHexagonalGrid();
    private void OnEnable() => Movement.OnUnitChangePosition += UpdateUnitPosition;
    private void OnDisable() => Movement.OnUnitChangePosition -= UpdateUnitPosition;

    private void GenerateHexagonalGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                var offsetX = (j % 2 == 0) ? i : i + .5f;
                var offsetY = .75f * j;

                // Create new hex object
                var position = new Vector3(offsetX, 0f, offsetY);
                var hex = Instantiate(hexPrefab, position, Quaternion.Euler(90f, 0f, 0f));
                hex.transform.SetParent(this.transform);
                hex.name = $"Hex {i} {j}";

                // Create hex boarder LineRenderer
                var line = hex.AddComponent<LineRenderer>();
                line.startWidth = .025f;
                line.loop = true;
                line.positionCount = 6;
                line.material = nodeMaterial;

                line.SetPosition(0, new Vector3(0f + offsetX, 0f, .5f + offsetY) + transform.position);
                line.SetPosition(1, new Vector3(.5f + offsetX, 0f, .25f + offsetY) + transform.position);
                line.SetPosition(2, new Vector3(.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(3, new Vector3(0f + offsetX, 0f, -.5f + offsetY) + transform.position);
                line.SetPosition(4, new Vector3(-.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(5, new Vector3(-.5f + offsetX, 0f, .25f + offsetY) + transform.position);

                // Add created node to list
                gridNodes.Add(hex.GetComponent<GridNode>());
            }
        }
    }

    private void GenerateSquareGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                // Create new hex object
                var position = new Vector3(i, 0f, j);
                var square = Instantiate(hexPrefab, position, Quaternion.Euler(90f, 0f, 0f));
                square.transform.SetParent(this.transform);
                square.name = $"Square {i} {j}";

                // Create hex boarder LineRenderer
                var line = square.AddComponent<LineRenderer>();
                line.startWidth = .025f;
                line.loop = true;
                line.positionCount = 4;
                line.material = nodeMaterial;

                line.SetPosition(0, new Vector3(.5f + i, 0f, .5f + j) + transform.position);
                line.SetPosition(1, new Vector3(.5f + i, 0f, -.5f + j) + transform.position);
                line.SetPosition(2, new Vector3(-.5f + i, 0f, -.5f + j) + transform.position);
                line.SetPosition(3, new Vector3(-.5f + i, 0f, .5f + j) + transform.position);

                // Add created node to list
                gridNodes.Add(square.GetComponent<GridNode>());
            }
        }
    }

    public void UpdateUnitPosition(Unit movingUnit, GridNode nextPosition)
    {
        var oldPosition = gridNodes.Find(position => position.unit == movingUnit);
        var newPosition = nextPosition;

        oldPosition.isOccupied = false;
        oldPosition.unit = null;

        newPosition.isOccupied = true;
        newPosition.unit = movingUnit;
    }
}