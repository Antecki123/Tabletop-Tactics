using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridBuilder))]
[RequireComponent(typeof(SceneryBuilder))]
[RequireComponent(typeof(UnitsSpawner))]
public class BattlefieldCreator : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GridBuilder gridBuilder;
    [SerializeField] private SceneryBuilder sceneryBuilder;
    [SerializeField] private UnitsSpawner unitSpawner;
    [Space]
    [SerializeField] private TurnManager turnManager;

    private Queue<IMapBuilder> battlefieldComponents = new();

    private void OnEnable()
    {
        gridBuilder.OnComplete += ExecuteCommand;
        sceneryBuilder.OnComplete += ExecuteCommand;
        unitSpawner.OnComplete += ExecuteCommand;
    }
    private void OnDisable()
    {
        gridBuilder.OnComplete -= ExecuteCommand;
        sceneryBuilder.OnComplete -= ExecuteCommand;
        unitSpawner.OnComplete -= ExecuteCommand;
    }

    private void Start()
    {
        battlefieldComponents.Enqueue(gridBuilder);
        battlefieldComponents.Enqueue(unitSpawner);
        battlefieldComponents.Enqueue(sceneryBuilder);

        ExecuteCommand();
    }

    private void ExecuteCommand()
    {
        if (battlefieldComponents.Count > 0)
            battlefieldComponents.Dequeue().Execute();
        else
            turnManager.enabled = true;
    }
}