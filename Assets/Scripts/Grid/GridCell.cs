using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("Node Attributes")]
    private LineRenderer line;
    private List<GridCell> adjacentCells = new();

    #region A* Pathfinding
    public float GCost { get; set; }
    public float HCost { get; set; }
    public float FCost { get { return GCost + HCost; } }
    public GridCell Connection { get; set; }
    public int GetDistance(GridCell targetNode)
    {
        return (int)Mathf.Round(Vector3.Distance(transform.position, targetNode.transform.position) * 10);
    }
    #endregion

    #region PROPERTIES
    [field: SerializeField] public Unit Unit { get; set; }
    [field: SerializeField] public bool IsOccupied { get; set; }
    [field: SerializeField] public Vector2Int Coordinates { get; set; }
    [field: SerializeField] public int MovementValue { get; set; }

    public List<GridCell> AdjacentCells { get => adjacentCells; private set => adjacentCells = value; }
    #endregion

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        Invoke(nameof(GetAdjacentBlocks), 3);

        MovementValue = -1;
    }

    private void GetAdjacentBlocks()
    {
        int gridMask = 1024;
        float overlapRange = .75f;

        var overlappedBlocks = Physics.OverlapSphere(transform.position, overlapRange, gridMask);

        foreach (var block in overlappedBlocks)
            AdjacentCells.Add(block.GetComponent<GridCell>());

        AdjacentCells.Remove(this);
        print("Node loaded.");
    }
}