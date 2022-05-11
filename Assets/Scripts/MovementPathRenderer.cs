using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementPathRenderer : MonoBehaviour
{
    [Header("Component References")]
    private AStarPathfinding pathfinding;
    private LineRenderer line;
    private Camera mainCamera;

    private bool isActive;
    private GridCell originPoint;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        pathfinding = new();
    }

    public void TurnOn(GridCell origin)
    {
        originPoint = origin;
        isActive = true;
    }

    public void TurnOff()
    {
        originPoint = null;
        isActive = false;
    }

    private void Update()
    {
        CalculatePath();
    }

    private void CalculatePath()
    {
        var mouseHit = MousePosition();

        if (isActive && mouseHit)
        {
            var path = pathfinding.FindPath(originPoint, mouseHit).ToArray();
            for (int i = 0; i < path.Length; i++)
                path[i] += Vector3.up * .5f;

            line.enabled = true;
            line.positionCount = path.Length;
            line.SetPositions(path);
        }
        else
            line.enabled = false;
    }

    private GridCell MousePosition()
    {
        int layerMask = 1024;

        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, layerMask);
        return hit.collider.GetComponent<GridCell>();
    }
}
