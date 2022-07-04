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

    public async void UpdateObstaclesOnGrid()
    {
        await System.Threading.Tasks.Task.Delay(500);

        var nodesList = FindObjectsOfType<GridCell>();
        foreach (var node in nodesList)
        {
            if (Physics.Raycast(node.transform.position + Vector3.down * .1f, Vector3.up, out RaycastHit hit))
            {
                GridCellsList.Remove(node);
                Destroy(node.gameObject);
            }
        }
    }

    public async void UpdateUnitsOnGrid()
    {
        var unitsLayer = 512;
        await System.Threading.Tasks.Task.Delay(500);

        foreach (var node in GridCellsList)
        {
            if (Physics.Raycast(node.transform.position + Vector3.down * .1f, Vector3.up, out RaycastHit hit, 1f, unitsLayer))
            {
                node.Unit = hit.collider.GetComponent<Unit>();
                node.IsOccupied = true;
            }
        }
    }

    public void CalculateMaxMovementRange(GridCell startNode, int range)
    {
        var movementList = new List<GridCell>() { startNode };
        var bufforList = new List<GridCell>();

        ClearMovementValues();
        startNode.MovementValue = 0;

        for (int i = 1; i <= range; i++)
        {
            foreach (var node in movementList)
            {
                foreach (var adjacentCells in node.AdjacentCells.Where(a => !a.IsOccupied && a.MovementValue < 0))
                {
                    adjacentCells.MovementValue = i;
                    bufforList.Add(adjacentCells);
                }
            }

            foreach (var node in bufforList.Where(n => !movementList.Contains(n)))
                movementList.Add(node);

            bufforList.Clear();
        }
    }

    public void ClearMovementValues()
    {
        foreach (var node in GridCellsList.Where(n => n.MovementValue >= 0))
            node.MovementValue = -1;
    }
}