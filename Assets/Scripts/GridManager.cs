using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour, IHighlightGrid
{
    public static GridManager instance;

    [Header("Grid Properties")]
    [SerializeField] private List<GridNode> gridNodes = new();

    public List<GridNode> GridNodes { get => gridNodes; }

    private List<GridNode> adjacentBlocks = new();
    private bool isHighlighted = false;

    private readonly int gridMask = 1024;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        var nodes = FindObjectsOfType<GridNode>();
        foreach (var node in nodes)
            gridNodes.Add(node);
    }

    private void OnEnable()
    {
        Movement.OnUnitChangePosition += UpdateUnitPosition;
        Unit.OnDeath += RemoveUnitFromGrid;
    }
    private void OnDisable()
    {
        Movement.OnUnitChangePosition -= UpdateUnitPosition;
        Unit.OnDeath -= RemoveUnitFromGrid;
    }

    private void UpdateUnitPosition(Unit movingUnit, GridNode nextPosition)
    {
        var oldPosition = gridNodes.Find(position => position.unit == movingUnit);
        var newPosition = nextPosition;

        oldPosition.isOccupied = false;
        oldPosition.unit = null;

        newPosition.isOccupied = true;
        newPosition.unit = movingUnit;
    }

    private void RemoveUnitFromGrid(Unit removedUnit)
    {
        var position = gridNodes.Find(pos => pos.unit == removedUnit);
        position.unit = null;
        position.isOccupied = false;
    }

    // =========  IIHighlightGrid  =================================
    public void HighlightGridMovement(Unit unit, int range, Color color)
    {
        ClearHighlight();

        if (!isHighlighted)
        {
            isHighlighted = true;

            var centerNode = gridNodes.Find(node => node.unit == unit);
            centerNode.movementValue = 99;

            var centerNodeLine = centerNode.GetComponent<LineRenderer>();
            centerNodeLine.enabled = true;
            centerNodeLine.startColor = color;
            centerNodeLine.endColor = color;

            adjacentBlocks.Add(centerNode);

            for (int i = 1; i <= range; i++)
            {
                List<GridNode> bufforList = new();

                foreach (var block in adjacentBlocks)
                {
                    var overlappedBlocks = Physics.OverlapSphere(block.transform.position, 1, gridMask);

                    foreach (var overlapped in overlappedBlocks)
                    {
                        var overlappedComponent = overlapped.GetComponent<GridNode>();
                        if (overlappedComponent.movementValue == 0 && !overlappedComponent.isOccupied)
                        {
                            overlappedComponent.movementValue = i;

                            var nodeLine = overlapped.GetComponent<LineRenderer>();
                            nodeLine.enabled = true;
                            nodeLine.startColor = color;
                            nodeLine.endColor = color;

                            bufforList.Add(overlappedComponent);
                        }
                    }
                }

                foreach (var block in bufforList)
                    adjacentBlocks.Add(block);

                bufforList.Clear();
            }
        }
    }

    public void HighlightGridRange(Unit unit, int range, Color color)
    {
        ClearHighlight();

        if (!isHighlighted)
        {
            isHighlighted = true;

            var centerNode = gridNodes.Find(node => node.unit == unit);
            var overlappedBlocks = Physics.OverlapSphere(centerNode.transform.position, range, gridMask);

            foreach (var overlapped in overlappedBlocks)
            {
                var nodeLine = overlapped.GetComponent<LineRenderer>();
                nodeLine.enabled = true;
                nodeLine.startColor = color;
                nodeLine.endColor = color;
            }
        }
    }

    public void ClearHighlight()
    {
        isHighlighted = false;
        adjacentBlocks.Clear();

        foreach (var node in gridNodes)
        {
            var nodeLine = node.GetComponent<LineRenderer>();
            nodeLine.enabled = false;

            node.movementValue = 0;
        }
    }
}