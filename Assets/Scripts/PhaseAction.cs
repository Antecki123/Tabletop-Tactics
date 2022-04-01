using UnityEngine;

public class PhaseAction : MonoBehaviour
{
    private enum UnitAction { None, MeleeAttack, RangeAttack, Support, BlowHorn, Defence }

    [Header("Component References")]
    [SerializeField] private UnitAction activeAction;

    private Camera mainCamera;
    [SerializeField] private Unit activeUnit;

    private RangeAttack rangeAttack;
    private MeleeAttack meleeAtack;
    private Support support;

    private void Awake()
    {
        mainCamera = Camera.main;

        rangeAttack = new RangeAttack();
        meleeAtack = new MeleeAttack();
        support = new Support();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && activeAction == UnitAction.None)
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
        if (activeAction == UnitAction.RangeAttack)
            rangeAttack.UpdateAction(activeUnit);

        else if (activeAction == UnitAction.MeleeAttack)
            meleeAtack.UpdateAction();

        else if (activeAction == UnitAction.Support)
            support.UpdateAction();
    }

    public void MeleeAttack() => activeAction = (activeUnit) ? UnitAction.MeleeAttack : UnitAction.None;
    public void RangeAttack() => activeAction = (activeUnit) ? UnitAction.RangeAttack : UnitAction.None;
    public void Support() => activeAction = (activeUnit) ? UnitAction.Support : UnitAction.None;
    public void BlowHorn() => activeAction = (activeUnit) ? UnitAction.BlowHorn : UnitAction.None;
    public void Defence() => activeAction = (activeUnit) ? UnitAction.Defence : UnitAction.None;
}