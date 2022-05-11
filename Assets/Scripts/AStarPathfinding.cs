using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    public List<Vector3> FindPath(GridCell originNode, GridCell targetNode)
    {
        List<GridCell> openSet = new();
        HashSet<GridCell> closedSet = new();

        openSet.Add(originNode);

        while (openSet.Count > 0)
        {
            GridCell currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost &&
                    openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return ConvertListToVector3(closedSet);
            
            foreach (var adjacent in currentNode.AdjacentCells)
            {
                if (adjacent.IsOccupied || closedSet.Contains(adjacent))
                    continue;

                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, adjacent);
                if (newMovementCostToNeighbour < adjacent.GCost || !openSet.Contains(adjacent))
                {
                    adjacent.GCost = newMovementCostToNeighbour;
                    adjacent.HCost = GetDistance(adjacent, targetNode);

                    if (!openSet.Contains(adjacent))
                    {
                        openSet.Add(adjacent);
                    }
                }
            }
        }

        return ConvertListToVector3(closedSet);
    }

    private int GetDistance(GridCell currentNode, GridCell targetNode)
    {
        return (int)Math.Round(Vector3.Distance(currentNode.transform.position, targetNode.transform.position));
    }

    private List<Vector3> ConvertListToVector3(HashSet<GridCell> hashSetCollection)
    {
        List<Vector3> positions = new();

        foreach (var item in hashSetCollection)
            positions.Add(item.transform.position);

        return positions;
    }
}