using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIUnitController : MonoBehaviour
{
    public Unit activeUnit;
    public QueueBehavior queueBehavior;

    public void ExecuteAction()
    {
        activeUnit = queueBehavior.ActiveUnit;

        // if unit has a range reapon
        if (activeUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None)
        {
            // find the closest enemy

        }
    }

    private Unit FindClosestEnemies()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        List<EnemyUnit> enemyUnits = new();

        foreach (var enemy in allUnits.Where(t=> t.UnitOwner != activeUnit.UnitOwner))
        {
            var enemyUnit = new EnemyUnit()
            {
                unit = enemy,
                distance = Vector3.Distance(activeUnit.transform.position, enemy.transform.position)
            };

        }
        enemyUnits.OrderBy(e => e.distance);

        return enemyUnits.First().unit;
    }
}

public struct EnemyUnit
{
    public Unit unit;
    public float distance;
}