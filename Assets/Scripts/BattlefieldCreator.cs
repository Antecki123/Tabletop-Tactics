using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(GridBuilder))]
[RequireComponent(typeof(SceneryBuilder))]
[RequireComponent(typeof(UnitSpawner))]
public class BattlefieldCreator : MonoBehaviour
{
    public static BattlefieldCreator instance;

    public Dictionary<MapSize, Vector2Int> mapSizeList = new()
    {
        { MapSize.S, new Vector2Int(20, 20) },
        { MapSize.M, new Vector2Int(40, 40) },
        { MapSize.L, new Vector2Int(80, 80) },
        { MapSize.XL, new Vector2Int(100, 100) },
        { MapSize.XXL, new Vector2Int(150, 150) },
    };

    [Header("Component References")]
    private Queue<IMapBuilder> builderComponents = new();
    [SerializeField] private TurnManager turnManager;

    private GridBuilder gridBuilder;
    private SceneryBuilder sceneryBuilder;
    private UnitSpawner unitSpawner;

    [Header("Grid Builder")]
    public Transform gridTransform;
    public GameObject hexPrefab;
    [Space]
    public MapSize mapSize;

    [Header("Scenery Builder")]
    public List<GameObject> scenery;

    [Header("Units Spawner")]
    public Transform unitsTransform;
    public List<UnitsToSpawn> unitsList = new();
    public GameObject unitModel;

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

    private void Awake()
    {
        gridBuilder = GetComponent<GridBuilder>();
        sceneryBuilder = GetComponent<SceneryBuilder>();
        unitSpawner = GetComponent<UnitSpawner>();
    }

    private void Start()
    {
        builderComponents.Enqueue(gridBuilder);
        builderComponents.Enqueue(sceneryBuilder);
        builderComponents.Enqueue(unitSpawner);

        ExecuteCommand();
    }

    private async void ExecuteCommand()
    {
        await Task.Delay(1000);

        if (builderComponents.Count > 0)
            builderComponents.Dequeue().Execute(this);
        else
            turnManager.enabled = true;
    }
}

public enum MapSize { S, M, L, XL, XXL }