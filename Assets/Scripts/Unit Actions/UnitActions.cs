using System;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public static Action<Unit> OnFinishAction;

    [Header("Component References")]
    [SerializeField] private QueueBehavior queueBehavior;
    public AStarPathfinding pathfinding;

    [Header("Actions States")]
    private Movement movement;
    private RangeAttack rangeAttack;
    private MeleeAttack meleeAttack;
    private Guard guard;
    private CastSpell castSpell;
    
    public Unit ActiveUnit { get => queueBehavior.UnitsQueue[0]; }

    private void Start()
    {
        movement = GetComponent<Movement>();
        rangeAttack = GetComponent<RangeAttack>();
        meleeAttack = GetComponent<MeleeAttack>();
        guard = GetComponent<Guard>();
        castSpell = GetComponent<CastSpell>();
    }

    public void FinishAction()
    {
        //Debug.Log("FINISH Action");
        ActiveUnit.Action = Unit.CurrentAction.None;

        if (ActiveUnit.UnitActions == 0)
            OnFinishAction?.Invoke(ActiveUnit);
    }

    #region UI Action Buttons
    public void Movement() => movement.enabled = true;
    public void RangeAttack() => rangeAttack.enabled = true;
    public void MeleeAttack() => meleeAttack.enabled = true;
    public void Guard() => guard.enabled = true;
    public void CastSpell() => castSpell.enabled = true;

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