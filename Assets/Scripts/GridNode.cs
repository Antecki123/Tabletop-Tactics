using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private bool isOccupied;
    [SerializeField] private Vector2Int position;
    [Space]
    [SerializeField] private int movementValue = 0;

    #region PROPERTIES
    public Unit Unit { get => unit; set => unit = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public Vector2Int Position { get => position; set => position = value; }
    public int MovementValue { get => movementValue; set => movementValue = value; }
    #endregion

    public void HighlightNode(Color color, int movementValue)
    {
        var lineComponent = GetComponent<LineRenderer>();
        lineComponent.enabled = true;
        lineComponent.startColor = color;
        lineComponent.endColor = color;

        lineComponent.startWidth = .05f;
        lineComponent.endWidth = .05f;

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, .05f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        MovementValue = movementValue;
    }

    public void CheckIsOnOutside()
    {
        bool[] borderNumber = new bool[6];
        /*
        var grid = GridManager.instance.GridNodes;

        var border0 = grid.Find(node => (node.Position.x == this.Position.x && node.Position.y == this.Position.y + 1));
        if (border0.movementValue == 0)
            borderNumber[0] = true;
        */

        var nodeLine = GetComponent<LineRenderer>();

        for (int i = 0; i < borderNumber.Length; i++)
        {
            if (borderNumber[i] == true && i != borderNumber.Length - 1)
            {
                var startPosition = new Vector3(nodeLine.GetPosition(i).x, .2f, nodeLine.GetPosition(i).z);
                var endPosition = new Vector3(nodeLine.GetPosition(i + 1).x, .2f, nodeLine.GetPosition(i + 1).z);
                nodeLine.SetPosition(i, startPosition);
                nodeLine.SetPosition(i + 1, endPosition);
            }
            else if (borderNumber[i] == true && i == borderNumber.Length - 1)
            {
                var startPosition = new Vector3(nodeLine.GetPosition(i).x, .2f, nodeLine.GetPosition(i).z);
                var endPosition = new Vector3(nodeLine.GetPosition(0).x, .2f, nodeLine.GetPosition(0).z);
                nodeLine.SetPosition(i, startPosition);
                nodeLine.SetPosition(0, endPosition);
            }
        }
        
    }

    public void ClearHighlight()
    {
        var lineComponent = GetComponent<LineRenderer>();

        lineComponent.enabled = true;
        lineComponent.startColor = Color.white;
        lineComponent.endColor = Color.white;

        lineComponent.startWidth = .05f;
        lineComponent.endWidth = .05f;

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, 0f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        MovementValue = 0;
    }

    // ==============================
    //
    private void OnMouseOver()
    {
        var lineComponent = GetComponent<LineRenderer>();

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, .08f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        /*if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
        {
            lineComponent.enabled = true;
            lineComponent.startColor = Color.white;
            lineComponent.endColor = Color.white;
        }*/
    }

    private void OnMouseExit()
    {
        var lineComponent = GetComponent<LineRenderer>();

        for (int i = 0; i < lineComponent.positionCount; i++)
        {
            var newPosition = new Vector3(lineComponent.GetPosition(i).x, .05f, lineComponent.GetPosition(i).z);
            lineComponent.SetPosition(i, newPosition);
        }

        /*if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
            lineComponent.enabled = false;*/
    }
}