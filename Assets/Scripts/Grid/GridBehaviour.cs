using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GridManager gridManager;

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    public void CalculateMaxMovementRange(GridCell startNode, int range)
    {
        List<GridCell> movementList = new() { startNode };
        List<GridCell> bufforList = new();

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

    public void CaclulateRange()
    {

    }

    public void ClearMovementValues()
    {
        foreach (var node in gridManager.GridNodes.Where(n => n.MovementValue >= 0))
            node.MovementValue = -1;
    }
}