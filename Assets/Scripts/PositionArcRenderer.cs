using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PositionArcRenderer : MonoBehaviour
{
    [Header("Arc Attributes")]
    [SerializeField, Range(1, 100)] int resolution;

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
        ActivateLineRenderer();
    }

    private void ActivateLineRenderer()
    {
        if (isActive)
        {
            line.enabled = true;
            CalculateArc();
        }
        else
            line.enabled = false;
    }

    private void CalculateArc()
    {
        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / resolution)
        {
            var tangentLineVertex1 = Vector3.Lerp(originPoint.position, MiddlePosition(), ratio);
            var tangentLineVertex2 = Vector3.Lerp(MiddlePosition(), MousePosition(), ratio);

            var bazierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bazierPoint);
        }

        line.positionCount = pointList.Count;
        line.SetPositions(pointList.ToArray());
    }

    private Vector3 MiddlePosition()
    {
        var posX = ((MousePosition().x - originPoint.position.x) / 2) + originPoint.position.x;
        var posY = Vector3.Distance(originPoint.position, MousePosition()) / 2;
        var posZ = ((MousePosition().z - originPoint.position.z) / 2) + originPoint.position.z;

        return new Vector3(posX, posY, posZ);
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}