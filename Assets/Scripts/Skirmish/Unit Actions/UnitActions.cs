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

    [Header("UI References")]
    [SerializeField] private UIActionButtons UIActionButtons;
    [Space]
    [SerializeField] private GameEvent OnUpdateActions;
    [SerializeField] private BoolVariable movementButtonAvailable;
    [SerializeField] private BoolVariable rangeAttackButtonAvailable;
    [SerializeField] private BoolVariable singleAttackButtonAvailable;
    [SerializeField] private BoolVariable guardButtonAvailable;
    [SerializeField] private BoolVariable healButtonAvailable;

    [Header("Actions States")]
    [SerializeField] private Movement movement;
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private SingleAttack singleAttack;
    [SerializeField] private MultipleAttack multipleAttack;
    [SerializeField] private Guard guard;
    [SerializeField] private Heal heal;
    [SerializeField] private Berserk berserk;
    [SerializeField] private WeaponThrow weaponThrow;

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

    private void Start() => EnableAvailableActions();

    public void FinishAction()
    {
        State = UnitState.Idle;

        if (ActiveUnit.UnitActions <= 0)
            OnFinishAction?.Invoke(ActiveUnit);

        ActiveUnit.ResetStats();
        EnableAvailableActions();
    }

    private void EnableAvailableActions()
    {
        movementButtonAvailable.value = ActiveUnit.UnitActions > 0;

        rangeAttackButtonAvailable.value = ActiveUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None;
        singleAttackButtonAvailable.value = ActiveUnit.UnitActions > 0;
        guardButtonAvailable.value = ActiveUnit.UnitActions > 0;
        //healButtonAvailable.value = ActiveUnit.UnitClass == Unit.Class.Medic;

        OnUpdateActions?.Invoke();
    }

    #region UI Action Buttons
    public void Movement() => movement.enabled = true;
    public void RangeAttack() => rangeAttack.enabled = true;
    public void MeleeAttack() => singleAttack.enabled = true;
    public void Guard() => guard.enabled = true;
    public void CastSpell() => heal.enabled = true;

    public void ClearActions()
    {
        if (State == UnitState.Idle)
        {
            movement.enabled = false;
            rangeAttack.enabled = false;
            singleAttack.enabled = false;
            guard.enabled = false;
            heal.enabled = false;
        }
    }
    #endregion
}