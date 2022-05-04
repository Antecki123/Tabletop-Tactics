using UnityEngine;
using System;

public class UnitActions : MonoBehaviour
{
    public static Action<Unit> OnFinishAction;

    public enum UnitAction { None, MeleeAttack, RangeAttack, Support, BlowHorn, Defence, Movement, Wait }

    [Header("Component References")]
    [SerializeField] private QueueBehavior queueBehavior;
    [Space]
    [SerializeField] private UnitAction activeAction;
    [SerializeField] private Unit activeUnit;

    [Header("Actions")]
    private Movement movement;
    private RangeAttack rangeAttack;
    private MeleeAttack meleeAtack;
    private BlowHorn blowHorn;
    private Guard guard;

    public UnitAction ActiveAction { get => activeAction; private set => activeAction = value; }
    public Unit ActiveUnit { get => activeUnit; set => activeUnit = value; }

    private void Awake()
    {
        movement = new Movement(this);
        rangeAttack = new RangeAttack(this);
        meleeAtack = new MeleeAttack(this);
        blowHorn = new BlowHorn(this);
        guard = new Guard(this);
    }

    private void Update()
    {
        // Set active unit
        if (!ActiveUnit && queueBehavior.UnitsQueue.Count != 0)
            ActiveUnit = queueBehavior.UnitsQueue[0];

        // Select active action script
        if (ActiveAction == UnitAction.RangeAttack && ActiveUnit.UnitActions > 0)
            rangeAttack.UpdateAction();

        else if (ActiveAction == UnitAction.MeleeAttack && ActiveUnit.UnitActions > 0)
            meleeAtack.UpdateAction();

        else if (ActiveAction == UnitAction.Defence && ActiveUnit.UnitActions > 0)
            guard.UpdateAction();

        else if (ActiveAction == UnitAction.Movement && ActiveUnit.UnitActions > 0)
            movement.UpdateAction();

        else if (ActiveAction == UnitAction.BlowHorn && ActiveUnit.UnitActions > 0)
            blowHorn.UpdateAction();
    }

    public void ClearAction()
    {
        ActiveAction = UnitAction.None;
        GridManager.instance.GetComponent<IHighlightGrid>().ClearHighlight();
    }

    public void FinishAction()
    {
        ClearAction();

        if (ActiveUnit.UnitActions == 0)
        {
            OnFinishAction?.Invoke(ActiveUnit);
            ActiveUnit = null;
        }
    }

    //public void Support() => ActiveAction = UnitAction.Support;

    public void Movement() => ActiveAction = UnitAction.Movement;
    public void RangeAttack() => ActiveAction = UnitAction.RangeAttack;
    public void MeleeAttack() => ActiveAction = UnitAction.MeleeAttack;
    public void BlowHorn() => ActiveAction = UnitAction.BlowHorn;
    public void Defence() => ActiveAction = UnitAction.Defence;
    public void Wait() => ActiveAction = UnitAction.Wait;
}