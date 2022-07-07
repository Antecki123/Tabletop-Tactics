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
    [Space]
    [SerializeField] private GameEvent OnUpdateActions;
    [Space]
    [SerializeField] private BoolVariable movementButtonAvailable;
    [SerializeField] private BoolVariable rangeAttackButtonAvailable;
    [SerializeField] private BoolVariable singleAttackButtonAvailable;
    [SerializeField] private BoolVariable guardButtonAvailable;
    //[SerializeField] private BoolVariable healButtonAvailable;

    [Header("Actions States")]
    [SerializeField] private Movement movement;
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private SingleAttack singleAttack;
    //[SerializeField] private MultipleAttack multipleAttack;
    [SerializeField] private Guard guard;
    [SerializeField] private Heal heal;
    //[SerializeField] private Berserk berserk;

    public Unit ActiveUnit { get => unitsQueue.ActiveUnit; }
    public UnitState State { get; set; }

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

    [ContextMenu("EnableAvailableActions")]
    public void EnableAvailableActions()
    {
        movementButtonAvailable.value = ActiveUnit.UnitActions > 0;
        rangeAttackButtonAvailable.value = ActiveUnit.UnitActions > 0 && ActiveUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None;
        
        OnUpdateActions?.Invoke();
    }

    #region UI Action Buttons
    public void Movement() => movement.enabled = (State == UnitState.Idle);
    public void RangeAttack() => rangeAttack.enabled = (State == UnitState.Idle);
    public void MeleeAttack() => singleAttack.enabled = (State == UnitState.Idle);
    public void Guard() => guard.enabled = (State == UnitState.Idle);
    public void CastSpell() => heal.enabled = (State == UnitState.Idle);

    public void ClearActions()
    {
        movement.enabled = false;
        rangeAttack.enabled = false;
        singleAttack.enabled = false;
        guard.enabled = false;
        heal.enabled = false;
    }
    #endregion
}