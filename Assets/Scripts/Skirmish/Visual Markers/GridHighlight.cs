using System.Linq;
using UnityEngine;

public class GridHighlight : MonoBehaviour, IGridHighlight
{
    [Header("Component References")]
    [SerializeField] private GridManager gridManager;

    [ContextMenu("Turn Off Highlight")]
    public void TurnOffHighlight()
    {
        var nodes = gridManager.GridCellsList;

        foreach (var node in nodes.Where(n => n.BlockValue > 0))
        {
            node.GetComponent<LineRenderer>().startColor = Color.black;
            node.GetComponent<LineRenderer>().endColor = Color.black;

            node.transform.position = new Vector3(node.transform.position.x, 0f, node.transform.position.z);
        }
    }

    public void TurnOnHighlightSimpleRange(GridCell startNode, int range)
    {
        var nodes = gridManager.GridCellsList;
        gridManager.CalculateMaxRange(startNode, range);

        foreach (var node in nodes.Where(n => n.BlockValue > 0))
        {
            node.GetComponent<LineRenderer>().startColor = Color.yellow;
            node.GetComponent<LineRenderer>().endColor = Color.yellow;

            node.transform.position = new Vector3(node.transform.position.x, .05f, node.transform.position.z);
        }
    }

    public void TurnOnHighlightMovement(GridCell startNode, int range, int actions)
    {
        var nodes = gridManager.GridCellsList;
        gridManager.CalculateMovementRange(startNode, range * actions);

        if (actions > 1)
        {
            foreach (var node in nodes.Where(n => n.BlockValue > 0))
            {
                node.GetComponent<LineRenderer>().startColor = Color.blue;
                node.GetComponent<LineRenderer>().endColor = Color.blue;

                node.transform.position = new Vector3(node.transform.position.x, .05f, node.transform.position.z);
            }
        }

        foreach (var node in nodes.Where(n => n.BlockValue > 0 && n.BlockValue <= range))
        {
            node.GetComponent<LineRenderer>().startColor = Color.cyan;
            node.GetComponent<LineRenderer>().endColor = Color.cyan;

            node.transform.position = new Vector3(node.transform.position.x, .07f, node.transform.position.z);
        }
    }
}