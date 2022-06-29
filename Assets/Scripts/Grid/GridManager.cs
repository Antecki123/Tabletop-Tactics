using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [Header("Grid Component References")]
    public GridBehaviour gridBehaviour;

    [Header("Grid Properties")]
    [SerializeField] private List<GridCell> gridNodes = new();

    public List<GridCell> GridNodes { get => gridNodes; }

    [field: SerializeField] public List<GridCell> GridCellsList { get; set; }
    [field: SerializeField] public Vector2Int GridDimensions { get; set; }

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
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

    private void UpdateUnitPosition(Unit movingUnit, GridCell newPosition)
    {
        var oldPosition = gridNodes.Find(position => position.Unit == movingUnit);

        oldPosition.IsOccupied = false;
        oldPosition.Unit = null;

        newPosition.IsOccupied = true;
        newPosition.Unit = movingUnit;
    }

    private void RemoveUnitFromGrid(Unit removedUnit)
    {
        var position = gridNodes.Find(pos => pos.Unit == removedUnit);
        position.Unit = null;
        position.IsOccupied = false;
    }

    public async void UpdateObstaclesOnGrid()
    {
        await System.Threading.Tasks.Task.Delay(500);

        var nodesList = FindObjectsOfType<GridCell>();
        foreach (var node in nodesList)
        {
            if (Physics.Raycast(node.transform.position + Vector3.down * .1f, Vector3.up, out RaycastHit hit))
            {
                gridNodes.Remove(node);
                Destroy(node.gameObject);
            }
        }
    }

    public async void UpdateUnitsOnGrid()
    {
        var unitsLayer = 512;
        await System.Threading.Tasks.Task.Delay(500);

        foreach (var node in gridNodes)
        {
            if (Physics.Raycast(node.transform.position + Vector3.down * .1f, Vector3.up, out RaycastHit hit, 1f, unitsLayer))
            {
                node.Unit = hit.collider.GetComponent<Unit>();
                node.IsOccupied = true;
            }
        }
    }
}