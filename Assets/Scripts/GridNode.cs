using System.Collections;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private bool isOccupied;
    [SerializeField] private Vector2Int position;
    [Space]
    [SerializeField] private int movementValue = 0;

    public Unit Unit { get => unit; set => unit = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public Vector2Int Position { get => position; set => position = value; }
    public int MovementValue { get => movementValue; set => movementValue = value; }

    public void HighlightNode(Color color)
    {
        var nodeLine = GetComponent<LineRenderer>();
        nodeLine.enabled = true;
        nodeLine.startColor = color;
        nodeLine.endColor = color;

        nodeLine.startWidth = .1f;
        nodeLine.endWidth = .1f;

        for (int i = 0; i < nodeLine.positionCount; i++)
        {
            var newPosition = new Vector3(nodeLine.GetPosition(i).x, .1f, nodeLine.GetPosition(i).z);
            nodeLine.SetPosition(i, newPosition);
        }
    }

    public void ClearHighlight()
    {
        var nodeLine = GetComponent<LineRenderer>();

        nodeLine.enabled = true;
        nodeLine.startColor = Color.white;
        nodeLine.endColor = Color.white;

        nodeLine.startWidth = .02f;
        nodeLine.endWidth = .02f;

        for (int i = 0; i < nodeLine.positionCount; i++)
        {
            var newPosition = new Vector3(nodeLine.GetPosition(i).x, 0f, nodeLine.GetPosition(i).z);
            nodeLine.SetPosition(i, newPosition);
        }

        MovementValue = 0;
    }

    /*
    private void OnMouseOver()
    {
        var lineComponent = GetComponent<LineRenderer>();

        if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
        {
            lineComponent.enabled = true;
            lineComponent.startColor = Color.white;
            lineComponent.endColor = Color.white;
        }
    }

    private void OnMouseExit()
    {
        var lineComponent = GetComponent<LineRenderer>();

        if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
            lineComponent.enabled = false;
    }
    */
}