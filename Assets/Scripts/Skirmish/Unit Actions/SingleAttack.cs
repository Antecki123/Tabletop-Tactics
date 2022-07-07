using UnityEngine;
using System;
using System.Threading.Tasks;

public class SingleAttack : MonoBehaviour
{
    #region Actions
    public static Action<Unit> OnAttackAnimation;
    public static Action<Unit> OnBlockAnimation;
    public static Action<Unit> OnAvoidAnimation;
    #endregion

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputsManager inputs;
    [Space]
    [SerializeField] private Camera mainCamera;

    private Unit targetUnit;

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell lastNode;

    private void OnEnable()
    {
        originNode = gridManager.GridCellsList.Find(n => n.Unit == unitActions.ActiveUnit);

        VisualEfects.Instance.GridHighlight.TurnOnHighlightSimpleRange(originNode, (int)unitActions.ActiveUnit.Wargear.combatWeapon.range);
    }

    private void OnDisable()
    {
        targetUnit = null;
        originNode = null;
        lastNode = null;

        VisualEfects.Instance.ArcMarker?.TurnOffMarker();
        VisualEfects.Instance.PositionMarker?.TurnOffMarker();
        VisualEfects.Instance.GridHighlight.TurnOffHighlight();
    }

    public void Update()
    {
        // Clear action
        if (inputs.RightMouseButton && unitActions.State == UnitActions.UnitState.Idle)
        {
            this.enabled = false;
            return;
        }

        // Find target (set pointer)
        if (unitActions.State == UnitActions.UnitState.Idle && (targetNode = GetTargetNode()) && targetNode.Unit &&
            targetNode.Unit.UnitOwner != unitActions.ActiveUnit.UnitOwner && targetNode.BlockValue > 0)
        {
            if (targetNode != lastNode)
            {
                lastNode = targetNode;

                VisualEfects.Instance.ArcMarker?.TurnOnMarker(originNode, targetNode);
                VisualEfects.Instance.PositionMarker?.TurnOnMarker(originNode, targetNode);
            }
        }
        else
        {
            lastNode = null;

            VisualEfects.Instance.ArcMarker?.TurnOffMarker();
            VisualEfects.Instance.PositionMarker?.TurnOffMarker();
            return;
        }

        // Attack target
        if (inputs.LeftMouseButton && unitActions.State == UnitActions.UnitState.Idle &&
            GetTargetNode() == targetNode && targetNode.BlockValue > 0)
        {
            if ((targetUnit = targetNode.Unit) && targetUnit.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                unitActions.State = UnitActions.UnitState.ExecutingAction;

                // Turn dueling units on each other's direction
                unitActions.ActiveUnit.transform.LookAt(targetUnit.transform.position);
                targetUnit.transform.LookAt(unitActions.ActiveUnit.transform.position);

                ExecuteAction();

                unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                unitActions.FinishAction();

                this.enabled = false;
                return;
            }
        }
    }

    private async void ExecuteAction()
    {
        // Calculating melee attack chance: 50% + difference between MeleeFight of both unit's values multiplying by 5
        var hitChance = 50 + (unitActions.ActiveUnit.GetMeleeFight() - targetUnit.GetMeleeFight()) * 5;
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = hitChance >= hitResult;

        Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        OnAttackAnimation?.Invoke(unitActions.ActiveUnit);

        // wait for animation delay in millis
        var animationDelay = 2000;
        await Task.Delay(animationDelay);

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(targetUnit.GetDefence(), unitActions.ActiveUnit.GetStrenght());
            if (woundTarget)
            {
                // Action when target has been wounded
                targetUnit.GetDamage(1);
            }
            else OnBlockAnimation?.Invoke(targetUnit);
        }
        else OnAvoidAnimation?.Invoke(targetUnit);
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