using System;
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
    private UnitActions unitActions;
    private Camera mainCamera;

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell lastNode;

    private void OnEnable()
    {
        unitActions = GetComponent<UnitActions>();
        mainCamera = Camera.main;

        originNode = GridManager.instance.GridNodes.Find(n => n.Unit == unitActions.ActiveUnit);

        GridManager.instance.gridBehaviour.CalculateMaxMovementRange(originNode, unitActions.ActiveUnit.UnitMove);
    }
    private void OnDisable()
    {
        originNode = null;
        targetNode = null;
        lastNode = null;

        VisualEfects.instace.MovementMarker?.TurnOffMarker();
        VisualEfects.instace.PositionMarker?.TurnOffMarker();

        GridManager.instance.gridBehaviour.ClearMovementValues();
    }

    private void Update()
    {
        // Clear action
        if (Input.GetMouseButtonDown(1) && unitActions.State == UnitActions.UnitState.Idle)
        {
            VisualEfects.instace.MovementMarker?.TurnOffMarker();
            VisualEfects.instace.PositionMarker?.TurnOffMarker();

            this.enabled = false;
            return;
        }

        // Find path (set visual path)
        if ((targetNode = GetTargetNode()) && !targetNode.IsOccupied && unitActions.State == UnitActions.UnitState.Idle)
        {
            if (targetNode != lastNode)
            {
                lastNode = targetNode;

                unitActions.pathfinding.FindPath(originNode, targetNode);

                VisualEfects.instace.MovementMarker?.TurnOnMarker(originNode, targetNode);
                VisualEfects.instace.PositionMarker?.TurnOnMarker(originNode, targetNode);
            }
        }
        else
        {
            lastNode = null;

            VisualEfects.instace.MovementMarker?.TurnOffMarker();
            VisualEfects.instace.PositionMarker?.TurnOffMarker();

            return;
        }

        // Set destination and start movement
        if (Input.GetMouseButtonDown(0) && GetTargetNode() == targetNode && targetNode.MovementValue > 0 &&
            unitActions.pathfinding.CurrentPath.Count > 0)
        {
            unitActions.State = UnitActions.UnitState.ExecutingAction;
            UnitMovement();

            OnUnitChangePosition?.Invoke(unitActions.ActiveUnit, targetNode);
            unitActions.ActiveUnit.ExecuteAction(1);
        }
    }

    private async void UnitMovement()
    {
        var path = unitActions.pathfinding.CurrentPath;
        var unit = unitActions.ActiveUnit;
        var velocity = 2f;

        for (int i = 1; i < path.Count; i++)
        {
            if (i < path.Count) unit.transform.LookAt(path[i]);
            while (Vector3.Distance(unit.transform.position, path[i]) >= .05f)
            {
                OnMovementAnimation?.Invoke(unit, true);

                unit.transform.position = Vector3.MoveTowards(unit.transform.position, path[i], velocity * Time.deltaTime);
                await System.Threading.Tasks.Task.Yield();
            }
        }
        OnMovementAnimation?.Invoke(unit, false);

        path.Clear();
        unitActions.FinishAction();
        this.enabled = false;
    }

    private GridCell GetTargetNode()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, 1024);

        if (hit.collider)
            return hit.collider.GetComponent<GridCell>();
        else
            return null;
    }
}