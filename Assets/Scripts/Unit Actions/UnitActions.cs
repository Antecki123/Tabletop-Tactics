using System;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    #region Actions
    public static Action<Unit> OnFinishAction;  // send info to units queue to pick next unit
    #endregion

    public enum UnitState { Idle, ExecutingAction }

    [Header("Component References")]
    [SerializeField] private UnitsQueue unitsQueue;
    [SerializeField] internal AStarPathfinding pathfinding;

    [Header("UI References")]
    [SerializeField] private UIActionButtons UIActionButtons;

    [Header("Actions States")]
    [SerializeField] private Movement movement;
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private MeleeAttack meleeAttack;
    [SerializeField] private Guard guard;
    [SerializeField] private CastSpell castSpell;

    public Unit ActiveUnit { get => unitsQueue.ActiveUnit; }
    [field: SerializeField] public UnitState State { get; set; }

    private void OnEnable()
    {
        UIActionButtons.OnClickMovementAction += Movement;
        UIActionButtons.OnClickRangeAttackAction += RangeAttack;
        UIActionButtons.OnClickMeleeAttackAction += MeleeAttack;
        UIActionButtons.OnClickGuardAction += Guard;
        UIActionButtons.OnClickCastSpellAction += CastSpell;

        UIActionButtons.OnClearAction += ClearActions;

    }
    private void OnDisable()
    {
        UIActionButtons.OnClickMovementAction -= Movement;
        UIActionButtons.OnClickRangeAttackAction -= RangeAttack;
        UIActionButtons.OnClickMeleeAttackAction -= MeleeAttack;
        UIActionButtons.OnClickGuardAction -= Guard;
        UIActionButtons.OnClickCastSpellAction -= CastSpell;

        UIActionButtons.OnClearAction -= ClearActions;
    }

    public void FinishAction()
    {
        //Debug.Log("FINISH Action");
        State = UnitState.Idle;

        if (ActiveUnit.UnitActions == 0)
            OnFinishAction?.Invoke(ActiveUnit);
    }

    #region UI Action Buttons
    public void Movement() => movement.enabled = (State == UnitState.Idle);
    public void RangeAttack() => rangeAttack.enabled = (State == UnitState.Idle);
    public void MeleeAttack() => meleeAttack.enabled = (State == UnitState.Idle);
    public void Guard() => guard.enabled = (State == UnitState.Idle);
    public void CastSpell() => castSpell.enabled = (State == UnitState.Idle);

    public void ClearActions()
    {
        movement.enabled = false;
        rangeAttack.enabled = false;
        meleeAttack.enabled = false;
        guard.enabled = false;
        castSpell.enabled = false;
    }
    #endregion
}