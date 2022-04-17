using UnityEngine;

public class PhaseActions : MonoBehaviour
{
    public enum UnitAction { None, MeleeAttack, RangeAttack, Support, BlowHorn, Defence, Movement }

    [Header("Component References")]
    public Unit activeUnit;
    public UnitAction activeAction;

    private Camera mainCamera;
    private int unitsLayer = 512;

    [Header("Actions")]
    private Movement movement;
    private MeleeAttack meleeAtack;
    private RangeAttack rangeAttack;
    private Support support;

    private void Awake()
    {
        mainCamera = Camera.main;

        movement = new Movement(this);
        rangeAttack = new RangeAttack(this);
        meleeAtack = new MeleeAttack(this);
        support = new Support();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, unitsLayer) && activeAction == UnitAction.None)
        {
            if (hit.transform.GetComponent<Unit>())
            {
                activeUnit = hit.transform.GetComponent<Unit>();
                if (activeUnit.UnitOwner != PhaseManager.instance.activePlayer)
                    activeUnit = null;
            }
        }

        // Clear active action and unit
        if (Input.GetMouseButtonDown(1))
        {
            activeUnit = null;
            activeAction = UnitAction.None;
        }

        // Go to active action script
        if (activeAction == UnitAction.RangeAttack && activeUnit)
            rangeAttack.UpdateAction();

        else if (activeAction == UnitAction.MeleeAttack && activeUnit)
            meleeAtack.UpdateAction();

        else if (activeAction == UnitAction.Support && activeUnit)
            support.UpdateAction();

        else if (activeAction == UnitAction.Movement && activeUnit)
            movement.UpdateAction();
    }

    public void MeleeAttack() => activeAction = (activeUnit) ? UnitAction.MeleeAttack : UnitAction.None;
    public void RangeAttack() => activeAction = (activeUnit) ? UnitAction.RangeAttack : UnitAction.None;
    public void Support() => activeAction = (activeUnit) ? UnitAction.Support : UnitAction.None;
    public void BlowHorn() => activeAction = (activeUnit) ? UnitAction.BlowHorn : UnitAction.None;
    public void Defence() => activeAction = (activeUnit) ? UnitAction.Defence : UnitAction.None;
    public void Movement() => activeAction = (activeUnit) ? UnitAction.Movement : UnitAction.None;
}