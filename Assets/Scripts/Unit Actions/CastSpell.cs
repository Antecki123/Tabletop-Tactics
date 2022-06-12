using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    #region Actions
    // Attack
    public static Action<Unit> OnCastSpell;
    // Turn on pointer
    public static Action<GridCell, GridCell> OnFindingTarget;
    // Turn off pointer
    public static Action OnClearAction;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;
    private Camera mainCamera;

    private GridCell originNode;
    private GridCell targetNode;

    private void OnEnable()
    {
        unitActions = GetComponent<UnitActions>();
        mainCamera = Camera.main;

        originNode = GridManager.instance.GridNodes.Find(n => n.Unit == unitActions.ActiveUnit);
    }
    private void OnDisable()
    {
        originNode = null;
        targetNode = null;

        OnClearAction?.Invoke();
    }
    private void Update()
    {
        // Clear action
        if (Input.GetMouseButtonDown(1) && unitActions.State ==  UnitActions.UnitState.Idle)
        {
            this.enabled = false;
            return;
        }

        // Find target (set pointer)
        if ((targetNode = GetTargetNode()) && unitActions.State == UnitActions.UnitState.Idle)
        {
            OnFindingTarget?.Invoke(originNode, targetNode);
        }
        else
        {
            OnClearAction?.Invoke();
            return;
        }

        if (Input.GetMouseButtonDown(0) && GetTargetNode() == targetNode && unitActions.State == UnitActions.UnitState.Idle)
        {
            print("Cast Spell");
        }
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
