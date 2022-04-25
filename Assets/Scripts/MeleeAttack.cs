using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack
{
    [Header("Component References")]
    private PhaseActions phaseAction;
    private Camera mainCamera;

    [Header("Attack Script")]
    private Unit target;

    private readonly float attackDistance = 1f;

    public MeleeAttack(PhaseActions phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        if (!phaseAction.ActiveUnit)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && phaseAction.ActiveUnit)
        {
            target = hit.transform.GetComponent<Unit>();

            if (hit.transform.CompareTag("Unit") && target.UnitOwner != phaseAction.ActiveUnit.UnitOwner)
            {
                if (phaseAction.ActiveUnit.DuelAvailable && WoundTest.IsPossibleToAttack(target.GetDefence(), phaseAction.ActiveUnit.GetStrenght()) &&
                    attackDistance >= Vector3.Distance(phaseAction.ActiveUnit.transform.position, target.transform.position))
                {
                    phaseAction.ActiveUnit.transform.LookAt(target.transform.position);
                    target.transform.LookAt(phaseAction.ActiveUnit.transform.position);
                    //activeUnit.duelAvailable = false;

                    AttackEffect();
                    ClearAction();
                }
                else Debug.Log("You can't attack enemy!");
            }
        }
    }

    private void AttackEffect()
    {
        // Calculating melee attack chance: 50% + difference between MeleeFight of both units value multiplying by 5
        var hitChance = 50 + (phaseAction.ActiveUnit.GetMeleeFight() - target.GetMeleeFight()) * 5;
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{phaseAction.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), phaseAction.ActiveUnit.RangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
            }
        }
    }

    private void ClearAction()
    {
        target = null;

        phaseAction.ClearActiveAction();
    }
}