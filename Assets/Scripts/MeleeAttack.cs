using UnityEngine;
using System;

public class MeleeAttack
{
    public static Action<Unit> OnAttack;
    public static Action<Unit> OnBlock;
    public static Action<Unit> OnAvoid;

    [Header("Component References")]
    private readonly UnitActions unitActions;
    private readonly Camera mainCamera;

    [Header("Attack Script")]
    private Unit target;

    private readonly float attackDistance = 1.25f;

    public MeleeAttack(UnitActions phaseAction)
    {
        this.unitActions = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Highlight grid
        GridManager.instance.GetComponent<IHighlightGrid>().HighlightGridRange(unitActions.ActiveUnit, (int)attackDistance, Color.red);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && unitActions.ActiveUnit)
        {
            target = hit.transform.GetComponent<Unit>();
            Debug.Log(Vector3.Distance(unitActions.ActiveUnit.transform.position, target.transform.position));
            if (hit.transform.CompareTag("Unit") && target.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                if (attackDistance >= Vector3.Distance(unitActions.ActiveUnit.transform.position, target.transform.position))
                {
                    unitActions.ActiveUnit.transform.LookAt(target.transform.position);
                    target.transform.LookAt(unitActions.ActiveUnit.transform.position);

                    OnAttack?.Invoke(unitActions.ActiveUnit);
                    AttackEffect();

                    //unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                    unitActions.ActiveUnit.ExecuteAction(1);
                    unitActions.FinishAction();
                }
                else Debug.Log("You can't attack enemy!");
            }
        }

        // Clear active action
        if (Input.GetMouseButtonDown(1))
            unitActions.ClearAction();
    }

    private void AttackEffect()
    {
        // Calculating melee attack chance: 50% + difference between MeleeFight of both unit's values multiplying by 5
        var hitChance = 50 + (unitActions.ActiveUnit.GetMeleeFight() - target.GetMeleeFight()) * 5;
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = hitChance >= hitResult;

        Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), unitActions.ActiveUnit.GetStrenght());
            if (woundTarget)
            {
                // Action when target has been wounded
                target.GetDamage(1);
            }
            else OnBlock?.Invoke(target);
        }
        else OnAvoid?.Invoke(target);
    }
}