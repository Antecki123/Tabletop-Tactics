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

    private List<GridNode> gridNodes = new();
    private List<GridNode> adjacentBlocks = new();
    private readonly int gridMask = 1024;

    private void Awake() => GenerateHexagonalGrid();

    private void OnEnable()
    {
        Movement.OnUnitChangePosition += UpdateUnitPosition;

        PhaseActions.OnHighlightGrid += HighlightNodes;
        PhaseActions.OnClearHighlightGrid += ClearHighlightedNodes;
    }
    private void OnDisable()
    {
        Movement.OnUnitChangePosition -= UpdateUnitPosition;

        PhaseActions.OnHighlightGrid -= HighlightNodes;
        PhaseActions.OnClearHighlightGrid -= ClearHighlightedNodes;
    }

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
                hex.name = $"{i} {j}";

                hex.GetComponent<GridNode>().position.x = j;
                hex.GetComponent<GridNode>().position.y = i;

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

    private void UpdateUnitPosition(Unit movingUnit, GridNode nextPosition)
    {
        var oldPosition = gridNodes.Find(position => position.unit == movingUnit);
        var newPosition = nextPosition;

        oldPosition.isOccupied = false;
        oldPosition.unit = null;

        newPosition.isOccupied = true;
        newPosition.unit = movingUnit;
    }

    private void HighlightNodes()
    {
        print("HIGHLIGHT");

        var phaseAction = PhaseActions.instance;

        var centerNode = gridNodes.Find(node => node.unit == phaseAction.ActiveUnit);
        var range = Mathf.FloorToInt(phaseAction.ActiveUnit.MoveLeft);

        centerNode.movementValue = 99;
        centerNode.GetComponent<Renderer>().material.color = Color.cyan;

        adjacentBlocks.Add(centerNode);

        for (int i = 1; i <= range; i++)
        {
            List<GridNode> bufforList = new();

            foreach (var block in adjacentBlocks)
            {
                var overlappedBlocks = Physics.OverlapSphere(block.transform.position, 1, gridMask);

                foreach (var overlapped in overlappedBlocks)
                {
                    var overlappedComponent = overlapped.GetComponent<GridNode>();
                    if (overlappedComponent.movementValue == 0 && !overlappedComponent.isOccupied)
                    {
                        overlappedComponent.movementValue = i;
                        overlapped.GetComponent<Renderer>().material.color = Color.cyan;

                        bufforList.Add(overlappedComponent);
                    }
                }
            }

            foreach (var block in bufforList)
                adjacentBlocks.Add(block);

            bufforList.Clear();
        }
    }

    private void ClearHighlightedNodes()
    {
        print("HIGHLIGHT CLEAR");

        adjacentBlocks.Clear();

        foreach (var node in gridNodes)
        {
            node.GetComponent<Renderer>().material.color = new Color32(0, 135, 0, 255);
            node.movementValue = 0;
        }
    }
}