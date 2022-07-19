using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsQueue : MonoBehaviour
{
    #region Actions
    public static Action OnTurnEnd;
    #endregion

    [field: SerializeField] public List<Unit> UnitsOrder { get; private set; }
    public Unit ActiveUnit { get => UnitsOrder.First(); }

    private List<Unit> secondaryList = new();

    private void OnEnable()
    {
        Unit.OnDeath += RemoveKilled;
        UnitActions.OnFinishAction += FinishedAction;
    }
    private void OnDisable()
    {
        Unit.OnDeath -= RemoveKilled;
        UnitActions.OnFinishAction -= FinishedAction;
    }

    private void Start()
    {
        UnitsOrder = new List<Unit>(Unit.UnitsList);

        UnitsOrder.Sort((a, b) => a.UnitSpeed.CompareTo(b.UnitSpeed));
        UnitsOrder.Reverse();
    }

    private void FinishedAction(Unit unit)
    {
        UnitsOrder.Remove(unit);
        secondaryList.Add(unit);

        if (UnitsOrder.Count == 0)
        {
            OnTurnEnd?.Invoke();

            secondaryList.Sort((a, b) => a.UnitSpeed.CompareTo(b.UnitSpeed));
            secondaryList.Reverse();

            UnitsOrder.AddRange(secondaryList);
            secondaryList.Clear();
        }
    }

    private void RemoveKilled(Unit unit) => UnitsOrder.RemoveAll(u => u == unit);
}