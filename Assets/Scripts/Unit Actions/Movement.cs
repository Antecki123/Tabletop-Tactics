using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Actions
    // Unit changed his position
    public static Action<Unit, GridCell> OnUnitChangePosition;
    // New path has been found
    public static Action OnNewPath;
    // Clear action
    public static Action OnClearAction;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;
    private Camera mainCamera;

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell bufforNode;

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

        GridManager.instance.gridBehaviour.ClearMovementValues();
    }

    private void Update()
    {
        // Clear action
        if (Input.GetMouseButtonDown(1) && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            this.enabled = false;
            return;
        }

        // Find path (set visual path)
        if ((targetNode = GetTargetNode()) && !targetNode.IsOccupied && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            if (targetNode != bufforNode)
            {
                bufforNode = targetNode;
                unitActions.pathfinding.FindPath(originNode, targetNode);
                OnNewPath?.Invoke();
            }
        }
        else
        {
            OnClearAction?.Invoke();
            return;
        }

        // Set destination and start movement
        if (Input.GetMouseButtonDown(0) && GetTargetNode() == targetNode && unitActions.ActiveUnit.Action == Unit.CurrentAction.None &&
            unitActions.pathfinding.CurrentPath.Count > 0 && targetNode.MovementValue > 0)
        {
            unitActions.ActiveUnit.Action = Unit.CurrentAction.Movement;
            UnitMovement();

            OnUnitChangePosition?.Invoke(unitActions.ActiveUnit, targetNode);
            unitActions.ActiveUnit.ExecuteAction(1);
        }
    }

    private async void UnitMovement()
    {
        var path = unitActions.pathfinding.CurrentPath;
        var unit = unitActions.ActiveUnit;
        var velocity = 6f;

        for (int i = 1; i < path.Count; i++)
        {
            if (i < path.Count) unit.transform.LookAt(path[i]);
            while (Vector3.Distance(unit.transform.position, path[i]) >= .3f)
            {
                //Debug.Log($"Distance to corner {i}: {Vector3.Distance(unit.transform.position, path[i])}.");

                unit.transform.position = Vector3.Lerp(unit.transform.position, path[i], velocity * Time.deltaTime);
                await System.Threading.Tasks.Task.Yield();
            }
            //Debug.Log($"Corner {i} reached.");
        }

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