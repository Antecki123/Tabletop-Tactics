using UnityEngine;
using System;

public class MeleeAttack : MonoBehaviour
{
    #region Actions
    public static Action<Unit> OnAttack;
    public static Action<Unit> OnBlock;
    public static Action<Unit> OnAvoid;

    // Turn on pointer
    public static Action<GridCell, GridCell> OnFindingTarget;
    // Turn off pointer
    public static Action OnClearAction;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;
    private Camera mainCamera;

    private Unit targetUnit;

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
        targetUnit = null;
        originNode = null;
        targetNode = null;

        OnClearAction?.Invoke();
    }

    public void Update()
    {
        // Clear action
        if (Input.GetMouseButtonDown(1) && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            this.enabled = false;
            return;
        }

        // Find target (set pointer)
        if ((targetNode = GetTargetNode()) && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            OnFindingTarget?.Invoke(originNode, targetNode);
        }
        else
        {
            OnClearAction?.Invoke();
            return;
        }

        // Attack target
        if (Input.GetMouseButtonDown(0) && GetTargetNode() == targetNode && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            if ((targetUnit = targetNode.Unit) && targetUnit.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                if (originNode.AdjacentCells.Contains(targetNode))
                {
                    unitActions.ActiveUnit.Action = Unit.CurrentAction.MeleeAttack;

                    // Turn dueling units on each other's direction
                    unitActions.ActiveUnit.transform.LookAt(targetUnit.transform.position);
                    targetUnit.transform.LookAt(unitActions.ActiveUnit.transform.position);

                    OnAttack?.Invoke(unitActions.ActiveUnit);
                    AttackEffect();

                    //unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                    unitActions.ActiveUnit.ExecuteAction(1);
                    unitActions.FinishAction();

                    this.enabled = false;
                    return;
                }
                else Debug.Log("You can't attack enemy. Enemy is too far!");
            }
        }
    }

    private void AttackEffect()
    {
        // Calculating melee attack chance: 50% + difference between MeleeFight of both unit's values multiplying by 5
        var hitChance = 50 + (unitActions.ActiveUnit.GetMeleeFight() - targetUnit.GetMeleeFight()) * 5;
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = hitChance >= hitResult;

        Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(targetUnit.GetDefence(), unitActions.ActiveUnit.GetStrenght());
            if (woundTarget)
            {
                // Action when target has been wounded
                targetUnit.GetDamage(1);
            }
            else OnBlock?.Invoke(targetUnit);
        }
        else OnAvoid?.Invoke(targetUnit);
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