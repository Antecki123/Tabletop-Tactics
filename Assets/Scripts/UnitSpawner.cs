using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    private Transform unitsTransform;
    private List<UnitsToSpawn> unitsStats = new();
    private GameObject unitModel;

    public void Execute(BattlefieldCreator manager)
    {
        unitsTransform = manager.unitsTransform;
        unitsStats = manager.unitsList;
        unitModel = manager.unitModel;

        InstantiatePlayerUnit();

        StartCoroutine(WaitForSpawnUnits());
    }

    public void Response() => OnComplete?.Invoke();

    private IEnumerator WaitForSpawnUnits()
    {
        while (FindObjectsOfType<Unit>().Length < unitsStats.Count)
            yield return new WaitForEndOfFrame();

        Debug.Log($"Units Loaded ({FindObjectsOfType<Unit>().Length})");
        GridManager.instance.UpdateUnitsOnGrid();

        Response();
    }

    private void InstantiatePlayerUnit()
    {
        for (int i = 0; i < unitsStats.Count; i++)
        {
            var newUnit = Instantiate(unitModel);
            newUnit.transform.SetParent(unitsTransform.transform);

            var unitComponent = newUnit.GetComponent<Unit>();
            unitComponent.UnitBaseStats = unitsStats[i].unitStats;
            unitComponent.UnitOwner = unitsStats[i].player;
            unitComponent.Wargear = unitsStats[i].wargear;

            if (unitComponent.UnitOwner == Unit.Player.Player1)
                newUnit.transform.position = FindEmptySlotPlayer(1);
            else if (unitComponent.UnitOwner == Unit.Player.Player2)
                newUnit.transform.position = FindEmptySlotPlayer(2);
        }
    }

    private Vector3 FindEmptySlotPlayer(int playerNumber)
    {
        foreach (var slot in GridManager.instance.GridNodes)
        {
            if (playerNumber == 1 && slot.Position.x == 0 && !slot.IsOccupied)
            {
                slot.IsOccupied = true;
                return slot.transform.position;
            }
            else if (playerNumber == 2 && slot.Position.x == GridManager.instance.GridDimensions.x - 1 && !slot.IsOccupied)
            {
                slot.IsOccupied = true;
                return slot.transform.position;
            }
        }
        return Vector3.zero;
    }
}

[System.Serializable]
public class UnitsToSpawn
{
    public string unitName;
    public UnitStats unitStats;
    public Unit.Player player;

    public Wargear wargear;
}