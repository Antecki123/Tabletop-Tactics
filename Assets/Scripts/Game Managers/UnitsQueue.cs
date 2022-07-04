using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Units Queue", menuName = "Scriptable Objects/Utilities/Units Queue")]

public class UnitsQueue : ScriptableObject
{
    public List<Unit> UnitsList { get; private set; }
    public Unit ActiveUnit { get => UnitsList.First();}

    private void OnEnable()
    {
        Unit.OnDeath += RemoveUnitFromQueue;
        UnitActions.OnFinishAction += RemoveUnitFromQueue;
    }
    private void OnDisable()
    {
        Unit.OnDeath -= RemoveUnitFromQueue;
        UnitActions.OnFinishAction -= RemoveUnitFromQueue;
    }

    public void CreateNewQueue(Unit[] allUnits)
    {
        UnitsList = allUnits.ToList();
        UnitsList.Sort((a, b) => a.UnitSpeed.CompareTo(b.UnitSpeed));
        UnitsList.RemoveAll(unit => unit.UnitWounds <= 0);
        UnitsList.Reverse();
    }

    private void RemoveUnitFromQueue(Unit removedUnit)
    {
        UnitsList.Remove(removedUnit);
    }
}