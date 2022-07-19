using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneryBuilder : MonoBehaviour, IMapBuilder
{
    public Action OnComplete;

    [Header("Component References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SkirmishData skirmishData;

    [Header("Environment Settings")]
    [SerializeField] private Transform obstaclesTransform;
    private List<GameObject> obstaclesList;
    private int obstaclesDensity;

    private readonly Vector3[] availableRotations = new Vector3[6] { new Vector3(0.0f,   0.0f, 0.0f),
                                                                     new Vector3(0.0f,  60.0f, 0.0f),
                                                                     new Vector3(0.0f, 120.0f, 0.0f),
                                                                     new Vector3(0.0f, 180.0f, 0.0f),
                                                                     new Vector3(0.0f, 240.0f, 0.0f),
                                                                     new Vector3(0.0f, 300.0f, 0.0f) };

    private float obstaclesToPlaceCount = 0;

    [ContextMenu("Execute")]
    public void Execute()
    {
        obstaclesDensity = skirmishData.obstaclesDensity;
        obstaclesList = new List<GameObject>(skirmishData.obstaclesList);

        obstaclesToPlaceCount = gridManager.GridCellsList.Count * obstaclesDensity * .01f;

        while (obstaclesToPlaceCount > 0)
            InstantiateObstacle();

        Debug.Log($"SCENERY LOADED.");
        Response();
    }

    public void Response() => OnComplete?.Invoke();

    private void InstantiateObstacle()
    {
        var newObstacle = Instantiate(obstaclesList[UnityEngine.Random.Range(0, obstaclesList.Count)]);

        newObstacle.transform.SetParent(obstaclesTransform);

        newObstacle.transform.SetPositionAndRotation(
            gridManager.GridCellsList[UnityEngine.Random.Range(0, gridManager.GridCellsList.Count)].transform.position,
            Quaternion.Euler(availableRotations[UnityEngine.Random.Range(0, 6)]));

        var obstacleColliders = newObstacle.GetComponentsInChildren<Collider>();

        foreach (var coll in obstacleColliders)
        {
            var raycastGrid = Physics.RaycastAll(coll.transform.position + Vector3.up, -coll.transform.up, 2.0f, 1024);
            for (int i = 0; i < raycastGrid.Length; i++)
            {
                var gridCell = raycastGrid[i].collider.GetComponent<GridCell>();
                var padding = 2;

                if (gridCell && !gridCell.IsOccupied && gridCell.Coordinates.x >= padding &&
                    gridCell.Coordinates.x < gridManager.GridDimensions.x - padding)
                {
                    gridCell.IsOccupied = true;
                    gridCell.GetComponent<LineRenderer>().enabled = false; //TODO: LINE RENDERER DISAPPEAR RANDOMLY
                }
                else
                {
                    Destroy(newObstacle);
                    return;
                }
                obstaclesToPlaceCount--;
            }
        }
    }
}