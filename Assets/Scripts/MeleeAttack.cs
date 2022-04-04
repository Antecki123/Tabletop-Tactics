using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack
{
    [Header("Component References")]
    [SerializeField] private PhaseAction phaseAction;
    [SerializeField] private Camera mainCamera;

    [Header("Attack Script")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private Unit target;

    private float attackDistance = 1f;

    public MeleeAttack(PhaseAction phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        if (!phaseAction.activeUnit)
            return;
        else if (!activeUnit)
            this.activeUnit = phaseAction.activeUnit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && activeUnit)
        {
            target = hit.transform.GetComponent<Unit>();

            if (hit.transform.CompareTag("Unit") && target.UnitOwner != activeUnit.UnitOwner)
            {
                if (phaseAction.activeUnit.duelAvailable && WoundTest.IsPossibleToAttack(target.GetDefence(), activeUnit.GetStrenght()) &&
                    attackDistance >= Vector3.Distance(phaseAction.activeUnit.transform.position, target.transform.position))
                {
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
        var hitChance = 50 + (activeUnit.unitMeleeFight - target.unitMeleeFight) * 5;
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{activeUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), activeUnit.RangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
            }
        }
    }

    private void ClearAction()
    {
        activeUnit = null;
        target = null;

        phaseAction.activeUnit = null;
        phaseAction.activeAction = PhaseAction.UnitAction.None;
    }
}