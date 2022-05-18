using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    [field: SerializeField] public List<Vector3> CurrentPath { get; private set; }

    public void FindPath(GridCell startNode, GridCell targetNode)
    {
        var toSearch = new List<GridCell>() { startNode };
        var processed = new List<GridCell>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.FCost < current.FCost || t.FCost == current.FCost && t.HCost < current.HCost) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            if (current == targetNode)
            {
                var currentPathTile = targetNode;
                var path = new List<GridCell>();
                var count = 100;

                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0)
                        throw new Exception();
                }

                path.Add(startNode);

                // Set new path
                CurrentPath = ConvertListToVector3(path);
            }

            foreach (var neighbor in current.AdjacentCells.Where(t => !t.IsOccupied && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbor);

                var costToNeighbor = current.GCost + current.GetDistance(neighbor);

                if (!inSearch || costToNeighbor < neighbor.GCost)
                {
                    neighbor.GCost = costToNeighbor;
                    neighbor.Connection = current;

                    if (!inSearch)
                    {
                        neighbor.HCost = neighbor.GetDistance(targetNode);
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
    }

    private List<Vector3> ConvertListToVector3(List<GridCell> path)
    {
        List<Vector3> positions = new();

        foreach (var item in path)
            positions.Add(item.transform.position);

        positions.Reverse();
        return positions;
    }
}