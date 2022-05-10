using UnityEngine;

public class SceneryBuilder : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    private int cellsAmount = 0;

    public void Execute(BattlefieldCreator manager)
    {
        cellsAmount = manager.mapSizeList[manager.mapSize].x * manager.mapSizeList[manager.mapSize].y;

        GridManager.instance.UpdateObstaclesOnGrid();

        Debug.Log($"Scenery Loaded");
        Response();
    }



    public void Response() => OnComplete?.Invoke();
}
