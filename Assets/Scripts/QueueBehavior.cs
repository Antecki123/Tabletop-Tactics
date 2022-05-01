using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueBehavior : MonoBehaviour
{
    [Header("Queue Properties")]
    [SerializeField] private List<Unit> unitsQueue = new();

    public List<Unit> UnitsQueue { get => unitsQueue; }

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
        unitsQueue = allUnits.ToList();
        unitsQueue.Sort((a, b) => a.UnitSpeed.CompareTo(b.UnitSpeed));
        unitsQueue.RemoveAll(unit => unit.UnitWounds <= 0);
        unitsQueue.Reverse();
    }

    private void RemoveUnitFromQueue(Unit removedUnit)
    {
        unitsQueue.Remove(removedUnit);
    }

    private void ChangePositionToLast(Unit unit)
    {
        var unitMoved = unitsQueue.Find(u => u == unit);
        unitsQueue.Remove(unitMoved);
        unitsQueue.Add(unitMoved);
    }
}