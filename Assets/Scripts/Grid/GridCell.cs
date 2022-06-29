using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
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
    //[field: SerializeField] public Obstacle Obstacle { get; set; }
    [field: SerializeField] public bool IsOccupied { get; set; }
    [field: SerializeField] public Vector2Int Coordinates { get; set; }
    [field: SerializeField] public int MovementValue { get; set; } = -1;

    [field: SerializeField] public HashSet<GridCell> AdjacentCells { get; private set; } = new();
    #endregion

    private void Start() => StartCoroutine(GetAdjacentBlocks());

    private IEnumerator GetAdjacentBlocks()
    {
        var gridMask = 1024;
        var overlapRange = .75f;

        yield return new WaitForEndOfFrame();

        var overlappedBlocks = Physics.OverlapSphere(transform.position, overlapRange, gridMask);
        foreach (var block in overlappedBlocks)
            AdjacentCells.Add(block.GetComponent<GridCell>());

        AdjacentCells.Remove(this);
    }
}