using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementPathRenderer : MonoBehaviour
{
    private LineRenderer line;
    private Camera mainCamera;

    private Transform originPoint;
    private bool isActive;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
    }

    public void TurnOn(Transform origin)
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
        if (isActive)
        {
            line.enabled = true;
            CalculatePath();
        }
        else
            line.enabled = false;
    }

    private void CalculatePath()
    {
        var pointList = new List<Vector3>();
        for (int i = 0; i < pointList.Count; i++)
        {

        }

        line.positionCount = pointList.Count;
        line.SetPositions(pointList.ToArray());
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}
