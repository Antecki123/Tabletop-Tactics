using UnityEngine;
using System;

public class PhaseActions : MonoBehaviour
{
    public static PhaseActions instance;

    public static Action OnHighlightGrid;
    public static Action OnClearHighlightGrid;

    public enum UnitAction { None, MeleeAttack, RangeAttack, Support, BlowHorn, Defence, Movement }

    [Header("Component References")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private UnitAction activeAction;

    private Camera mainCamera;
    private readonly int unitsLayer = 512;

    [Header("Actions")]
    private Movement movement;
    private MeleeAttack meleeAtack;
    private RangeAttack rangeAttack;
    private Guard guard;

    public Unit ActiveUnit { get => activeUnit; set => activeUnit = value; }
    public UnitAction ActiveAction { get => activeAction; private set => activeAction = value; }

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);

        mainCamera = Camera.main;

        movement = new Movement(this);
        rangeAttack = new RangeAttack(this);
        meleeAtack = new MeleeAttack(this);
        guard = new Guard(this);
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set active unit
        /*
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, unitsLayer) && ActiveAction == UnitAction.None)
        {
            if (hit.transform.GetComponent<Unit>())
            {
                ActiveUnit = hit.transform.GetComponent<Unit>();
                if (ActiveUnit.UnitOwner != PhaseManager.instance.ActivePlayer)
                    ActiveUnit = null;
            }
        }
        */

        // Clear active action and unit
        if (Input.GetMouseButtonDown(1))
            ClearActiveAction();

        // Go to active action script
        if (ActiveAction == UnitAction.RangeAttack && ActiveUnit)
            rangeAttack.UpdateAction();

        else if (ActiveAction == UnitAction.MeleeAttack && ActiveUnit)
            meleeAtack.UpdateAction();

        else if (ActiveAction == UnitAction.Defence && ActiveUnit)
            guard.UpdateAction();

        else if (ActiveAction == UnitAction.Movement && ActiveUnit)
            movement.UpdateAction();
    }

    public void ClearActiveAction()
    {
        ActiveUnit = null;
        ActiveAction = UnitAction.None;

        OnClearHighlightGrid?.Invoke();
    }

    public void MeleeAttack() => ActiveAction = (ActiveUnit) ? UnitAction.MeleeAttack : UnitAction.None;
    public void RangeAttack() => ActiveAction = (ActiveUnit) ? UnitAction.RangeAttack : UnitAction.None;
    public void Support() => ActiveAction = (ActiveUnit) ? UnitAction.Support : UnitAction.None;
    public void BlowHorn() => ActiveAction = (ActiveUnit) ? UnitAction.BlowHorn : UnitAction.None;
    public void Defence() => ActiveAction = (ActiveUnit) ? UnitAction.Defence : UnitAction.None;

    public void Movement()
    {
        OnHighlightGrid?.Invoke();
        ActiveAction = (ActiveUnit) ? UnitAction.Movement : UnitAction.None;
    }
}