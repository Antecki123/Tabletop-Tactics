using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("Node Properties")]
    [SerializeField] private Unit unit;
    [SerializeField] private bool isOccupied;
    [SerializeField] private Vector2Int coordinates;
    [SerializeField] private List<GridCell> adjacentCells = new();
    [Space]
    [SerializeField] private int movementValue = 0;     // to delete G value

    // ========================= IN DEVELOPMENT A*
    [SerializeField, Tooltip("Cost from start tile to this tile.")] private int gCost = 0;
    [SerializeField, Tooltip("Estimated cost from this tile to destination tile.")] private int hCost = 0;

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int FCost { get { return gCost + hCost; } }
    // ========================= IN DEVELOPMENT A*

    #region PROPERTIES
    public Unit Unit { get => unit; set => unit = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
    public int MovementValue { get => movementValue; set => movementValue = value; }        // to delete

    public List<GridCell> AdjacentCells { get => adjacentCells; set => adjacentCells = value; }
    #endregion

    private void Start() => Invoke("GetAdjacentBlocks", 5);

    private void GetAdjacentBlocks()
    {
        int gridMask = 1024;
        float overlapRange = .75f;

        var overlappedBlocks = Physics.OverlapSphere(transform.position, overlapRange, gridMask);

        foreach (var block in overlappedBlocks)
            adjacentCells.Add(block.GetComponent<GridCell>());

        adjacentCells.Remove(this);
    }

    public void HighlightNode(Color color, int movementValue)
    {
        var lineComponent = GetComponent<LineRenderer>();
        lineComponent.enabled = true;
        lineComponent.startColor = color;
        lineComponent.endColor = color;

        lineComponent.startWidth = .05f;
        lineComponent.endWidth = .05f;

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, .07f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        MovementValue = movementValue;
    }

    public void ClearHighlight()
    {
        var lineComponent = GetComponent<LineRenderer>();

        lineComponent.enabled = true;
        lineComponent.startColor = Color.black;
        lineComponent.endColor = Color.black;

        lineComponent.startWidth = .05f;
        lineComponent.endWidth = .05f;

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, .05f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        MovementValue = 0;
    }
}