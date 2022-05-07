using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LineRenderer borderLine;

    private List<GridCell> adjacentBlocks = new();
    private readonly int gridMask = 1024;

    private bool isHighlighted = false;

    public void HighlightGridMovement(Unit unit, int range, Color color)
    {
        ClearHighlight();

        isHighlighted = true;

        var centerNode = gridManager.GridNodes.Find(node => node.Unit == unit);
        centerNode.MovementValue = 99;

        var centerNodeLine = centerNode.GetComponent<LineRenderer>();
        centerNodeLine.enabled = true;
        centerNodeLine.startColor = color;
        centerNodeLine.endColor = color;

        adjacentBlocks.Add(centerNode);

        for (int i = 1; i <= range; i++)
        {
            List<GridCell> bufforList = new();

            foreach (var block in adjacentBlocks)
            {
                var overlappedBlocks = Physics.OverlapSphere(block.transform.position, 1, gridMask);

                foreach (var overlapped in overlappedBlocks)
                {
                    var overlappedComponent = overlapped.GetComponent<GridCell>();
                    if (overlappedComponent.MovementValue == 0 && !overlappedComponent.IsOccupied)
                    {
                        overlappedComponent.HighlightNode(color, i);
                        bufforList.Add(overlappedComponent);

                        //HighlightBorder(range);
                    }
                }
            }

            foreach (var block in bufforList)
                adjacentBlocks.Add(block);

            bufforList.Clear();
        }
    }

    public void HighlightGridRange(Unit unit, int range, Color color)
    {
        ClearHighlight();

        if (!isHighlighted)
        {
            isHighlighted = true;

            var centerNode = gridManager.GridNodes.Find(node => node.Unit == unit);
            var overlappedBlocks = Physics.OverlapSphere(centerNode.transform.position, range, gridMask);

            foreach (var overlapped in overlappedBlocks)
                overlapped.GetComponent<GridCell>().HighlightNode(color, 1);

        }
    }

    public void ClearHighlight()
    {
        isHighlighted = false;
        adjacentBlocks.Clear();

        foreach (var node in gridManager.GridNodes)
            node.ClearHighlight();
    }

    // IN DEVELOPMENT
    public void HighlightBorder(int range)
    {
        borderLine.startWidth = .07f;
        borderLine.endWidth = .07f;
        borderLine.loop = true;
        //borderLine.positionCount = 6;
        borderLine.useWorldSpace = false;

        GridCell[] outerBlocks = gridManager.GridNodes.ToArray();

        foreach (var block in outerBlocks)
        {
            if (block.MovementValue == range)
            {
                block.transform.position = new Vector3(block.transform.position.x, 0.5f, block.transform.position.z);
            }
        }
    }
}