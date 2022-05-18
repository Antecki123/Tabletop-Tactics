using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GridManager gridManager;

    private List<GridCell> highlightedBlocks = new();
    private readonly int gridMask = 1024;

    private bool isHighlighted = false;

    public void HighlightGridMovement(Unit unit, int range, Color color)
    {
        if (!isHighlighted)
        {
            ClearHighlight();
            isHighlighted = true;

            var centerNode = gridManager.GridNodes.Find(node => node.Unit == unit);
            centerNode.HighlightNode(color, 0);

            highlightedBlocks.Add(centerNode);

            for (int i = 1; i <= range; i++)
            {
                List<GridCell> bufforList = new();

                foreach (var block in highlightedBlocks)
                {
                    var overlappedBlocks = block.AdjacentCells;

                    foreach (var overlapped in overlappedBlocks)
                    {
                        if (overlapped.MovementValue == -1 && !overlapped.IsOccupied)
                        {
                            overlapped.HighlightNode(color, i);
                            bufforList.Add(overlapped);
                        }
                    }
                }

                foreach (var block in bufforList)
                    highlightedBlocks.Add(block);

                bufforList = null;
            }
        }
    }

    public void HighlightGridRange(Unit unit, int range, Color color)
    {
        ClearHighlight();

        if (!isHighlighted)
        {
            isHighlighted = true;

            var centerNode = gridManager.GridNodes.Find(node => node.Unit == unit);
            var overlappedBlocks = Physics.OverlapSphere(centerNode.transform.position, range * .75f, gridMask);

            foreach (var overlapped in overlappedBlocks)
                overlapped.GetComponent<GridCell>().HighlightNode(color, 1);

        }
    }

    public void ClearHighlight()
    {
        isHighlighted = false;
        highlightedBlocks.Clear();

        foreach (var node in gridManager.GridNodes)
            node.ClearHighlight();
    }
}