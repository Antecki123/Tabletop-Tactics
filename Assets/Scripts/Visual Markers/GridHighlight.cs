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

        foreach (var node in nodes.Where(n => n.MovementValue > 0))
        {
            node.GetComponent<LineRenderer>().startColor = Color.black;
            node.GetComponent<LineRenderer>().endColor = Color.black;

            node.transform.position = new Vector3(node.transform.position.x, 0f, node.transform.position.z);
        }
    }

    public void TurnOnHighlight(GridCell startNode, int range)
    {
        throw new System.NotImplementedException();
    }

    [ContextMenu("Highlight")]
    public void TurnOnHighlightMovement()
    {
        var nodes = gridManager.GridCellsList;

        foreach (var node in nodes.Where(n => n.MovementValue > 0))
        {
            node.GetComponent<LineRenderer>().startColor = Color.blue;
            node.GetComponent<LineRenderer>().endColor = Color.blue;

            node.transform.position = new Vector3(node.transform.position.x, .05f, node.transform.position.z);

        }
    }
}