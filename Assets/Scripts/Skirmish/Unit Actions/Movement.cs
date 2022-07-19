using System;
using System.Threading.Tasks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Actions
    // Send info to grid about new unit position
    public static Action<Unit, GridCell> OnUnitChangePosition;

    // Movement animation
    public static Action<Unit, bool> OnMovementAnimation;
    #endregion

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputsManager inputs;
    [SerializeField] private AStarPathfinding pathfinding;
    [Space]
    [SerializeField] private Camera mainCamera;

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell lastNode;

    private void OnEnable()
    {
        originNode = gridManager.GridCellsList.Find(o => o.Unit == unitActions.ActiveUnit);

        gridManager.CalculateMovementRange(originNode, originNode.Unit.UnitMove, originNode.Unit.UnitActions);
        VisualEfects.Instance.GridHighlight.TurnOnHighlightMovement(originNode, originNode.Unit.UnitMove, originNode.Unit.UnitActions);
    }

    private void OnDisable()
    {
        originNode = null;
        targetNode = null;
        lastNode = null;

        VisualEfects.Instance.MovementMarker?.TurnOffMarker();
        VisualEfects.Instance.PositionMarker?.TurnOffMarker();
        VisualEfects.Instance.GridHighlight?.TurnOffHighlight();
    }

    private void Update()
    {
        // Clear action
        if (inputs.RightMouseButton && unitActions.State == UnitActions.UnitState.Idle)
        {
            this.enabled = false;
            return;
        }

        // Find path (set visual path)
        if ((targetNode = GetTargetNode()) && !targetNode.IsOccupied && unitActions.State == UnitActions.UnitState.Idle)
        {
            if (targetNode != lastNode)
            {
                lastNode = targetNode;

                pathfinding.FindPath(originNode, targetNode);

                VisualEfects.Instance.MovementMarker?.TurnOnMarker(originNode, targetNode);
                VisualEfects.Instance.PositionMarker?.TurnOnMarker(originNode, targetNode);
            }
        }
        else
        {
            lastNode = null;

            VisualEfects.Instance.MovementMarker?.TurnOffMarker();
            VisualEfects.Instance.PositionMarker?.TurnOffMarker();

            return;
        }

        // Set destination and start movement
        if (inputs.LeftMouseButton && GetTargetNode() == targetNode && unitActions.State == UnitActions.UnitState.Idle &&
            targetNode.BlockValue > 0 && pathfinding.CurrentPath.Count > 0)
        {
            unitActions.State = UnitActions.UnitState.ExecutingAction;

            var actionPoints = (targetNode.BlockValue <= originNode.Unit.UnitMove) ? 1 : originNode.Unit.UnitActions;
            originNode.Unit.ExecuteAction(actionPoints);

            ExecuteAction();
            OnUnitChangePosition?.Invoke(originNode.Unit, targetNode);
        }
    }

    private async void ExecuteAction()
    {
        var path = pathfinding.CurrentPath;
        var unit = originNode.Unit;
        var velocity = 2f;

        for (int i = 1; i < path.Count; i++)
        {
            if (i < path.Count) unit.transform.LookAt(path[i]);
            while (Vector3.Distance(unit.transform.position, path[i]) >= .05f)
            {
                OnMovementAnimation?.Invoke(unit, true);

                unit.transform.position = Vector3.MoveTowards(unit.transform.position, path[i], velocity * Time.deltaTime);
                await Task.Yield();
            }
        }
        OnMovementAnimation?.Invoke(unit, false);

        path.Clear();
        unitActions.FinishAction();
        this.enabled = false;
    }

    private GridCell GetTargetNode()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(inputs.MousePosition), out RaycastHit hit, 100f, 1024);

        if (hit.collider)
            return hit.collider.GetComponent<GridCell>();
        else
            return null;
    }
}