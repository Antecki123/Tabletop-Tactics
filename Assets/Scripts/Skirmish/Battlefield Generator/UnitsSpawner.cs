using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour, IMapBuilder
{
    public Action OnComplete;

    [Header("Component References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SkirmishData skirmishData;

    [Header("Units Spawner Settings")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    private List<SkirmishData.UnitsToSpawn> unitsToSpawn;
    private int spawnedUnits = 0;

    public void Execute()
    {
        unitsToSpawn = new List<SkirmishData.UnitsToSpawn>(skirmishData.unitsToSpawn);

        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            InstantiateUnit(i);
        }

        StartCoroutine(WaitForSpawnUnits());
    }

    public void Response() => OnComplete?.Invoke();

    private IEnumerator WaitForSpawnUnits()
    {
        while (spawnedUnits < unitsToSpawn.Count)
            yield return new WaitForEndOfFrame();

        Debug.Log($"UNITS SPAWNED: {spawnedUnits} / {unitsToSpawn.Count} UNITS.");

        Response();
    }

    private void InstantiateUnit(int i)
    {
        GameObject newUnit;
        GridCell slotPosition;

        if (unitsToSpawn[i].player == Unit.Player.Player1)
        {
            slotPosition = FindEmptySlotPlayer(1);
            newUnit = Instantiate(unitsToSpawn[i].unitStats.unitModel, slotPosition.transform.position, Quaternion.Euler(0.0f, 90.0f, 0.0f));
            newUnit.transform.SetParent(player1);
        }
        else
        {
            slotPosition = FindEmptySlotPlayer(2);
            newUnit = Instantiate(unitsToSpawn[i].unitStats.unitModel, slotPosition.transform.position, Quaternion.Euler(0.0f, -90.0f, 0.0f));
            newUnit.transform.SetParent(player2);
        }

        var unitComponent = newUnit.GetComponent<Unit>();
        unitComponent.UnitBaseStats = unitsToSpawn[i].unitStats;
        unitComponent.UnitOwner = unitsToSpawn[i].player;

        unitComponent.UnitArmy = unitsToSpawn[i].unitArmy;
        unitComponent.UnitType = unitsToSpawn[i].unitType;
        unitComponent.UnitHeroicTier = unitsToSpawn[i].unitHeroicTier;
        unitComponent.UnitClass = unitsToSpawn[i].unitClass;

        unitComponent.Wargear = unitsToSpawn[i].wargear;

        slotPosition.Unit = unitComponent;
        slotPosition.IsOccupied = true;

        spawnedUnits++;
    }

    private GridCell FindEmptySlotPlayer(int playerNumber)
    {
        GridCell slotPosition;

        if (playerNumber == 1)
        {
            foreach (var slot in gridManager.GridCellsList
                .Where(s => !s.IsOccupied && s.Coordinates.x == 0)
                .Where(s => s.Coordinates.y % 2 == 0)
                .Reverse())
            {
                slotPosition = slot;
                return slotPosition;
            }
        }
        else
        {
            foreach (var slot in gridManager.GridCellsList
                .Where(s => !s.IsOccupied && s.Coordinates.x == gridManager.GridDimensions.x - 1)
                .Where(s => s.Coordinates.y % 2 != 0))
            {
                slotPosition = slot;
                return slotPosition;
            }
        }

        throw new NotImplementedException();
    }
}