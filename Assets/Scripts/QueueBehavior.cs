using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueBehavior : MonoBehaviour
{
    [field: SerializeField] public List<Unit> UnitsQueue { get; private set; }

    private void OnEnable()
    {
        Unit.OnDeath += RemoveUnitFromQueue;
        UnitActions.OnFinishAction += RemoveUnitFromQueue;
    }
    private void OnDisable()
    {
        Unit.OnDeath -= RemoveUnitFromQueue;
        UnitActions.OnFinishAction += RemoveUnitFromQueue;
    }

    public void CreateNewQueue(Unit[] allUnits)
    {
        UnitsQueue = allUnits.ToList();
        UnitsQueue.Sort((a, b) => a.UnitSpeed.CompareTo(b.UnitSpeed));
        UnitsQueue.RemoveAll(unit => unit.UnitWounds <= 0);
        UnitsQueue.Reverse();
    }

    private void RemoveUnitFromQueue(Unit removedUnit)
    {
        UnitsQueue.Remove(removedUnit);
    }
}