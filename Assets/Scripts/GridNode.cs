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