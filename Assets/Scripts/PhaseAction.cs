using UnityEngine;

public class PhaseAction : MonoBehaviour
{
    public enum UnitAction { None, MeleeAttack, RangeAttack, Support, BlowHorn, Defence }

    [Header("Component References")]
    public Unit activeUnit;
    public UnitAction activeAction;

    private Camera mainCamera;

    [SerializeField] private RangeAttack rangeAttack;
    private MeleeAttack meleeAtack;
    private Support support;

    private void Awake()
    {
        mainCamera = Camera.main;

        rangeAttack = new RangeAttack(this);
        meleeAtack = new MeleeAttack();
        support = new Support();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && activeAction == UnitAction.None)
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
    }

    public void MeleeAttack() => activeAction = (activeUnit) ? UnitAction.MeleeAttack : UnitAction.None;
    public void RangeAttack() => activeAction = (activeUnit) ? UnitAction.RangeAttack : UnitAction.None;
    public void Support() => activeAction = (activeUnit) ? UnitAction.Support : UnitAction.None;
    public void BlowHorn() => activeAction = (activeUnit) ? UnitAction.BlowHorn : UnitAction.None;
    public void Defence() => activeAction = (activeUnit) ? UnitAction.Defence : UnitAction.None;
}