using System;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public static Action<Unit> OnFinishAction;

    [Header("Component References")]
    [SerializeField] private QueueBehavior queueBehavior;
    public AStarPathfinding pathfinding;

    [Header("Actions States")]
    [SerializeField] private Movement movement;
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private MeleeAttack meleeAttack;
    
    public Unit ActiveUnit { get => queueBehavior.UnitsQueue[0]; }

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

    public void ClearActions()
    {
        movement.enabled = false;
        rangeAttack.enabled = false;
        meleeAttack.enabled = false;
    }
    #endregion
}