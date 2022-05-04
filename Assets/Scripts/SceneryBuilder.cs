using UnityEngine;

public class SceneryBuilder : MonoBehaviour, IMapBuilder
{
    public System.Action OnComplete;

    public void Execute(BattlefieldCreator manager)
    {
        Debug.Log($"Scenery Loaded");
        GridManager.instance.UpdateObstaclesOnGrid();

        Response();
    }

    public void Response() => OnComplete?.Invoke();
}
