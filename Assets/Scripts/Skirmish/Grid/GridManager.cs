using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [field: SerializeField] public List<GridCell> GridCellsList { get; set; }
    [field: SerializeField] public Vector2Int GridDimensions { get; set; }

    private void OnEnable()
    {
        Movement.OnUnitChangePosition += UpdateUnitPosition;
        Unit.OnDeath += RemoveUnitFromGrid;
    }
    private void OnDisable()
    {
        Movement.OnUnitChangePosition -= UpdateUnitPosition;
        Unit.OnDeath -= RemoveUnitFromGrid;
    }

    private void UpdateUnitPosition(Unit movingUnit, GridCell newPosition)
    {
        var oldPosition = GridCellsList.Find(p => p.Unit == movingUnit);

        oldPosition.IsOccupied = false;
        oldPosition.Unit = null;

        newPosition.IsOccupied = true;
        newPosition.Unit = movingUnit;
    }

    private void RemoveUnitFromGrid(Unit removedUnit)
    {
        var position = GridCellsList.Find(p => p.Unit == removedUnit);
        position.Unit = null;
        position.IsOccupied = false;
    }

    public void CalculateMovementRange(GridCell startNode, int range)
    {
        var movementList = new List<GridCell>() { startNode };
        var bufforList = new List<GridCell>();

        ClearMovementValues();
        startNode.BlockValue = 0;

        for (int i = 1; i <= range; i++)
        {
            foreach (var node in movementList)
            {
                foreach (var adjacentCells in node.AdjacentCells.Where(a => !a.IsOccupied && a.BlockValue < 0))
                {
                    adjacentCells.BlockValue = i;
                    bufforList.Add(adjacentCells);
                }
            }

            foreach (var node in bufforList.Where(n => !movementList.Contains(n)))
                movementList.Add(node);

            bufforList.Clear();
        }
    }

    public void CalculateMaxRange(GridCell startNode, int range)
    {
        var rangeList = new List<GridCell>() { startNode };
        var bufforList = new List<GridCell>();

        ClearMovementValues();
        startNode.BlockValue = 0;

        for (int i = 1; i <= range; i++)
        {
            foreach (var node in rangeList)
            {
                foreach (var adjacentCells in node.AdjacentCells.Where(a => a.BlockValue < 0))
                {
                    adjacentCells.BlockValue = i;
                    bufforList.Add(adjacentCells);
                }
            }

            foreach (var node in bufforList.Where(n => !rangeList.Contains(n)))
                rangeList.Add(node);

            bufforList.Clear();
        }
    }

    public void ClearMovementValues()
    {
        foreach (var node in GridCellsList.Where(n => n.BlockValue >= 0))
            node.BlockValue = -1;
    }
}